using System;
using System.Reflection;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Attributes;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jasily.Frameworks.Cli.Configures
{
    internal class TypeConfigure<TClass> : ITypeConfigure
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HashSet<Type> _inheritedTypes = new HashSet<Type>();
        private readonly ConcurrentDictionary<Type, ITypeConfigure> _inheritedsConfigures
            = new ConcurrentDictionary<Type, ITypeConfigure>();
        private readonly HashSet<PropertyInfo> _properties = new HashSet<PropertyInfo>();
        private readonly ConcurrentDictionary<PropertyInfo, PropertyConfigure> _propertiesConfigures
            = new ConcurrentDictionary<PropertyInfo, PropertyConfigure>();

        public TypeConfigure(IServiceProvider serviceProvider)
        {
            // init fields
            this._serviceProvider = serviceProvider;
            this._properties = new HashSet<PropertyInfo>(typeof(TClass).GetRuntimeProperties());

            // init properties
            this.TypeInfo = this.Type.GetTypeInfo();

            var n = new NameConfiguration();
            foreach (var attrubute in this.TypeInfo.GetCustomAttributes())
            {
                if (attrubute is CommandClassAttribute)
                {
                    this.HasCommandClassAttribute = true;
                }

                (attrubute as IConfigureableAttribute<INameConfiguration>)?.Apply(n);
            }
            this.Names = n.Names.ToArray().AsReadOnly();

            // load inherited
            var t = typeof(TClass);
            while (this._inheritedTypes.Add(t = t?.GetTypeInfo().BaseType)) { }
            this._inheritedTypes.Remove(null);
        }

        public Type Type { get; } = typeof(TClass);

        public TypeInfo TypeInfo { get; }

        public bool HasCommandClassAttribute { get; }

        public IReadOnlyList<string> Names { get; }

        #region ITypeConfigure

        public ITypeConfigure GetInheritedTypeConfigure(Type declaringType)
        {
            if (declaringType == this.Type) return this;
            if (!this._inheritedTypes.Contains(declaringType)) throw new InvalidOperationException();

            if (this._inheritedsConfigures.TryGetValue(declaringType, out var c)) return c;
            var configure = (ITypeConfigure)
                this._serviceProvider.GetRequiredService(typeof(TypeConfigure<>).MakeGenericType(declaringType));
            return this._inheritedsConfigures.GetOrAdd(declaringType, configure);
        }

        public IPropertyConfigure GetConfigure(PropertyInfo property)
        {
            if (!this._properties.Contains(property)) throw new InvalidOperationException();

            if (this._propertiesConfigures.TryGetValue(property, out var r))
            {
                return r;
            }

            if (property.DeclaringType != this.Type)
            {
                return this.GetInheritedTypeConfigure(property.DeclaringType).GetConfigure(property);
            }
            else
            {
                return this._propertiesConfigures.GetOrAdd(property, (_) => new PropertyConfigure(this._serviceProvider, property));
            }
        }

        #endregion        

        internal class PropertyConfigure : IPropertyConfigure
        {
            private readonly IServiceProvider _serviceProvider;

            public PropertyConfigure(IServiceProvider serviceProvider, PropertyInfo property)
            {
                this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                this.Property = property ?? throw new ArgumentNullException(nameof(property));

                if (property.GetCustomAttribute<CommandPropertyAttribute>() is CommandPropertyAttribute propertyAttribute)
                {
                    this.HasCommandPropertyAttribute = true;
                }
            }

            public PropertyInfo Property { get; }

            public bool HasCommandPropertyAttribute { get; }
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
