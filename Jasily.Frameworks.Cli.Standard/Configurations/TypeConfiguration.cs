using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Commands;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal class TypeConfiguration<TClass> : ITypeConfiguration
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, ITypeConfiguration> _inheritedsConfigures = new Dictionary<Type, ITypeConfiguration>();
        private readonly Dictionary<PropertyInfo, IPropertyConfiguration> _propertiesConfigurations;
        private readonly Dictionary<MethodInfo, IMethodConfiguration> _methodsConfigurations;

        public TypeConfiguration(IServiceProvider serviceProvider)
        {
            // init fields
            this._serviceProvider = serviceProvider;

            // load attributes
            this.TypeInfo = this.Type.GetTypeInfo();

            var n = new NameConfigurator();
            foreach (var attrubute in this.TypeInfo.GetCustomAttributes())
            {
                if (attrubute is CommandClassAttribute)
                {
                    this.IsDefinedCommand = true;
                }

                (attrubute as IConfigureableAttribute<INameConfigurator>)?.Apply(n);
            }
            this.Names = n.BuildName(this.Type.Name);
            

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

            // load sub commands
            var cmds = new List<ICommand>();
            cmds.AddRange(this._propertiesConfigurations
                .Select(z=> z.Value.TryMakeCommand(typeof(TClass)))
                .Where(z => z != null));
            cmds.AddRange(this._methodsConfigurations
                .Select(z => z.Value.TryMakeCommand(typeof(TClass)))
                .Where(z => z != null));
            this.AvailableCommands = cmds.AsReadOnly();
        }

        public Type Type { get; } = typeof(TClass);

        public TypeInfo TypeInfo { get; }

        public bool IsDefinedCommand { get; }

        public IReadOnlyList<string> Names { get; }

        #region ITypeConfiguration

        public ITypeConfiguration GetInheritedTypeConfigure(Type declaringType)
        {
            if (this._inheritedsConfigures.TryGetValue(declaringType, out var v)) return v;
            throw new InvalidOperationException();
        }

        public IPropertyConfiguration GetConfigure(PropertyInfo property)
        {
            if (this._propertiesConfigurations.TryGetValue(property, out var v)) return v;
            throw new InvalidOperationException();
        }

        public IReadOnlyList<ICommand> AvailableCommands { get; }

        #endregion

        private IPropertyConfiguration BuildConfiguration(PropertyInfo property)
        {
            return new PropertyConfiguration(this, property);
        }

        private IMethodConfiguration BuildConfiguration(MethodInfo method)
        {
            return new MethodConfiguration(this, method);
        }

        internal class PropertyConfiguration : IPropertyConfiguration
        {
            public PropertyConfiguration(TypeConfiguration<TClass> typeConfiguration, PropertyInfo property)
            {
                this.ServiceProvider = typeConfiguration._serviceProvider;
                this.Property = property ?? throw new ArgumentNullException(nameof(property));

                var n = new NameConfigurator();
                foreach (var attrubute in property.GetCustomAttributes())
                {
                    if (attrubute is CommandPropertyAttribute)
                    {
                        this.IsDefinedCommand = true;
                    }

                    (attrubute as IConfigureableAttribute<INameConfigurator>)?.Apply(n);
                }
                this.Names = n.BuildName(property.Name);
            }

            public IServiceProvider ServiceProvider { get; }

            public IReadOnlyList<string> Names { get; }

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

        internal class MethodConfiguration : IMethodConfiguration
        {
            public MethodConfiguration(TypeConfiguration<TClass> typeConfiguration, MethodInfo method)
            {
                this.ServiceProvider = typeConfiguration._serviceProvider;
                this.Method = method ?? throw new ArgumentNullException(nameof(method));

                var n = new NameConfigurator();
                foreach (var attrubute in method.GetCustomAttributes())
                {
                    if (attrubute is CommandMethodAttribute)
                    {
                        this.IsDefinedCommand = true;
                    }

                    (attrubute as IConfigureableAttribute<INameConfigurator>)?.Apply(n);
                }
                this.Names = n.BuildName(method.Name);
            }

            public IServiceProvider ServiceProvider { get; }

            public IReadOnlyList<string> Names { get; }

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
        }

        internal class ParameterAttribute
        {
            private readonly IServiceProvider _serviceProvider;

            public ParameterAttribute(IServiceProvider serviceProvider, ParameterInfo parameter)
            {
                this._serviceProvider = serviceProvider;
            }
        }
    }
}
