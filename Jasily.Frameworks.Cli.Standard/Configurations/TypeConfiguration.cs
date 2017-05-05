using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Jasily.Core;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Converters;
using Jasily.Frameworks.Cli.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal sealed class TypeConfiguration<TClass> : BaseConfiguration, ITypeConfiguration
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, ITypeConfiguration> _inheritedsConfigures = new Dictionary<Type, ITypeConfiguration>();
        private readonly Dictionary<PropertyInfo, IPropertyConfiguration> _propertiesConfigurations;
        private readonly Dictionary<MethodInfo, IMethodConfiguration> _methodsConfigurations;
        private IReadOnlyList<ICommand> _availableCommands;

        public TypeConfiguration(IServiceProvider serviceProvider)
        {
            // init fields
            this._serviceProvider = serviceProvider;

            // load attributes
            var attributes = typeof(TClass).GetTypeInfo().GetCustomAttributes().ToArray();
            this.Configure(attributes, typeof(TClass).Name);
            this.IsDefinedCommand = attributes.OfType<CommandClassAttribute>().FirstOrDefault() != null;

            // load inherited class
            var t = typeof(TClass);
            while ((t = t.GetTypeInfo().BaseType) != null)
            {
                var configure = (ITypeConfiguration)
                    this._serviceProvider.GetRequiredService(typeof(TypeConfiguration<>).FastMakeGenericType(t));
                this._inheritedsConfigures.Add(t, configure);
            }

            // load properties
            this._propertiesConfigurations = typeof(TClass).GetRuntimeProperties()
                .Select(this.BuildConfiguration)
                .ToDictionary(z => z.Property);
            this._methodsConfigurations = typeof(TClass).GetRuntimeMethods()
                .Select(this.BuildConfiguration)
                .ToDictionary(z => z.Method);
        }

        protected override void Configure(Attribute[] attributes, string declaringName)
        {
            base.Configure(attributes, declaringName);

            var cc = ConfigureConfigurator(attributes, new CommandClassConfigurator());
            this.CanBeResult = !cc.IsNotResult;
        }

        public bool IsDefinedCommand { get; }

        public bool CanBeResult { get; private set; }

        #region ITypeConfiguration

        public ITypeConfiguration GetInheritedTypeConfigure(Type declaringType)
        {
            if (this._inheritedsConfigures.TryGetValue(declaringType, out var v)) return v;
            throw new InvalidOperationException();
        }

        public IPropertyConfiguration GetConfiguration(PropertyInfo property)
        {
            if (this._propertiesConfigurations.TryGetValue(property, out var v)) return v;
            throw new InvalidOperationException();
        }

        public IMethodConfiguration GetConfiguration(MethodInfo method)
        {
            if (this._methodsConfigurations.TryGetValue(method, out var v)) return v;
            throw new InvalidOperationException();
        }

        public IReadOnlyList<ICommand> AvailableCommands
        {
            get
            {
                if (this._availableCommands == null)
                {
                    var cmds = new List<ICommand>();
                    cmds.AddRange(this._propertiesConfigurations
                        .Select(z => z.Value.TryMakeCommand(typeof(TClass)))
                        .Where(z => z != null));
                    cmds.AddRange(this._methodsConfigurations
                        .Select(z => z.Value.TryMakeCommand(typeof(TClass)))
                        .Where(z => z != null));
                    Interlocked.CompareExchange(ref this._availableCommands, cmds.AsReadOnly(), null);
                }
                return this._availableCommands;
            }
        }

        #endregion

        private IPropertyConfiguration BuildConfiguration(PropertyInfo property)
        {
            return new PropertyConfiguration(this, property);
        }

        private IMethodConfiguration BuildConfiguration(MethodInfo method)
        {
            return new MethodConfiguration(this, method);
        }

        internal sealed class PropertyConfiguration : BaseConfiguration, IPropertyConfiguration
        {
            public PropertyConfiguration(TypeConfiguration<TClass> typeConfiguration, PropertyInfo property)
            {
                this.ServiceProvider = typeConfiguration._serviceProvider;
                this.Property = property ?? throw new ArgumentNullException(nameof(property));

                var attributes = property.GetCustomAttributes().ToArray();
                this.Configure(attributes, property.Name);
                this.IsDefinedCommand = attributes.OfType<CommandPropertyAttribute>().FirstOrDefault() != null;
            }

            public IServiceProvider ServiceProvider { get; }

            public PropertyInfo Property { get; }

            public bool IsDefinedCommand { get; }

            public BaseCommand TryMakeCommand(Type type)
            {
                // only create public method getter
                if (!this.Property.CanRead || !this.Property.GetMethod.IsPublic) return null;
                // NOT static
                if (this.Property.GetMethod.IsStatic) return null;
                // only create NOT INDEX method
                if (this.Property.GetIndexParameters().Length > 0) return null;
                // inherit method will not create
                if (this.Property.DeclaringType != type && !this.IsDefinedCommand)
                {
                    return null;
                }

                return new PropertyCommand<TClass>(this);
            }
        }

        internal sealed class MethodConfiguration : BaseConfiguration, IMethodConfiguration
        {
            private IReadOnlyList<ParameterConfiguration> _parameterConfigurations;

            public MethodConfiguration(TypeConfiguration<TClass> typeConfiguration, MethodInfo method)
            {
                this.ServiceProvider = typeConfiguration._serviceProvider;
                this.Method = method ?? throw new ArgumentNullException(nameof(method));
                
                var attributes = method.GetCustomAttributes().ToArray();
                this.Configure(attributes, method.Name);
                this.IsDefinedCommand = attributes.OfType<CommandMethodAttribute>().FirstOrDefault() != null;
            }

            public IServiceProvider ServiceProvider { get; }

            public MethodInfo Method { get; }

            public bool IsDefinedCommand { get; }

            [CanBeNull]
            public BaseCommand TryMakeCommand(Type type)
            {
                if (!this.Method.IsPublic || this.Method.IsStatic)
                {
                    return null;
                }

                if (this.Method.DeclaringType != type && !this.IsDefinedCommand)
                {
                    return null;
                }
                
                return new MethodCommand<TClass>(this);
            }

            public IReadOnlyList<IParameterConfiguration> ParameterConfigurations
            {
                get
                {
                    if (this._parameterConfigurations == null)
                    {
                        var comaprer = this.ServiceProvider.GetRequiredService<StringComparer>();
                        var conflictNameMap = new Dictionary<string, ParameterInfo>(comaprer);
                        var parameters = this.Method.GetParameters()
                            .Select(z => new ParameterConfiguration(this, z, conflictNameMap, comaprer))
                            .ToArray()
                            .AsReadOnly();
                        Interlocked.CompareExchange(ref this._parameterConfigurations, parameters, null);
                    }

                    return this._parameterConfigurations;
                }
            }
        }

        internal sealed class ParameterConfiguration : BaseConfiguration, IParameterConfiguration
        {
            private readonly HashSet<string> _nameSet;
            private readonly List<ICondition> _conditions= new List<ICondition>();

            internal ParameterConfiguration(MethodConfiguration method, ParameterInfo parameter,
                Dictionary<string, ParameterInfo> conflictNameMap, StringComparer comaprer)
            {
                this.ParameterInfo = parameter;
                this.IsOptional = parameter.HasDefaultValue;
                this.IsResolveByEngine = method.ServiceProvider.GetRequiredService<AutoResolvedTypes>()
                    .Contains(parameter.ParameterType);

                var attributes = parameter.GetCustomAttributes().ToArray();
                this.Configure(attributes, parameter.Name);
                this._nameSet = new HashSet<string>(this.Names);
                foreach (var name in this._nameSet)
                {
                    if (conflictNameMap.TryGetValue(name, out var p))
                    {
                        var msg = new StringBuilder()
                            .AppendLine($"Parameter Definition Error on {typeof(TClass).Name}.{method.Method.Name}:")
                            .AppendLine($"   Name <{name}> is Conflict with Another Parameter <{p.Name}>.");
                        throw new InvalidOperationException(msg.ToString());
                    }
                    conflictNameMap.Add(name, parameter);
                }

                IValueConverter GetValueConverter(Type type)
                {
                    var converter = method.ServiceProvider.GetService(typeof(Converters.IValueConverter<>).FastMakeGenericType(type));
                    if (converter == null)
                    {
                        var msg = $"convert string {this.ParameterInfo.ParameterType.Name} is not supported.";
                        throw new InvalidOperationException(msg);
                    }
                    return (IValueConverter)converter;
                }

                if (this.ParameterInfo.ParameterType.IsArray)
                {
                    this.IsArray = true;
                    this.ArrayElementType = this.ParameterInfo.ParameterType.GetElementType();
                    this.ValueConverter = GetValueConverter(this.ArrayElementType);
                }
                else
                {
                    if (!this.IsResolveByEngine)
                    {
                        this.ValueConverter = GetValueConverter(this.ParameterInfo.ParameterType);
                    }

                    if (this.ParameterInfo.ParameterType == typeof(bool))
                    {
                        var configurator = new BooleanParameterConfigurator(comaprer);
                        attributes.OfType<IConfigureableAttribute<IBooleanParameterConfigurator>>()
                            .ForEach(z => z.Apply(configurator));
                        var boolMap = configurator.CreateMap();

                        if (this.ValueConverter != null)
                        {
                            this.ValueConverter = new MapedValueConverter(boolMap, this.ValueConverter);
                        }
                    }
                }
            }

            protected override void Configure(Attribute[] attributes, string declaringName)
            {
                base.Configure(attributes, declaringName);

                var pc = ConfigureConfigurator(attributes, new ParameterConfigurator());
                this._conditions.AddRange(pc.Conditions.ToArray());
            }

            public ParameterInfo ParameterInfo { get; }

            public IValueConverter ValueConverter { get; }

            public bool IsResolveByEngine { get; }

            public bool IsMatchName(string name)
            {
                return this._nameSet.Contains(name);
            }

            public void Check(object value) => this._conditions.ForEach(z => z.Check(value));

            #region array typed parameter

            /// <summary>
            /// is parameter accept mulit-value.
            /// </summary>
            public bool IsArray { get; }

            public Type ArrayElementType { get; }

            public IReadOnlyList<string> Conditions => 
                this._conditions.Select(z => z.ToString()).Where(z => !string.IsNullOrWhiteSpace(z))
                .ToArray();

            #endregion

            public bool IsOptional { get; }

            private class MapedValueConverter : IValueConverter
            {
                private readonly IValueConverter _baseValueConverter;
                private readonly IReadOnlyDictionary<string, string> _map;

                public MapedValueConverter(IReadOnlyDictionary<string, string> map, IValueConverter baseValueConverter)
                {
                    this._map = map;
                    this._baseValueConverter = baseValueConverter;
                }

                public object Convert(IEnumerable<string> values)
                {
                    return this._baseValueConverter.Convert(values.Select(this.Select));
                }

                private string Select(string value)
                {
                    return this._map.TryGetValue(value, out var r) ? r : value;
                }

                public object Convert(string value)
                {
                    return this._baseValueConverter.Convert(this.Select(value));
                }
            }
        }
    }
}
