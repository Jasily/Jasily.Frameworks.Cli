using System;
using System.Reflection;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Attributes;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Commands
{
    internal class TypeConfigure<TClass> : ITypeConfigure
    {
        private readonly IServiceProvider serviceProvider;
        private readonly HashSet<Type> inheritedTypes = new HashSet<Type>();
        private readonly ConcurrentDictionary<Type, ITypeConfigure> inheritedsConfigures
            = new ConcurrentDictionary<Type, ITypeConfigure>();
        private readonly HashSet<PropertyInfo> properties = new HashSet<PropertyInfo>();
        private readonly ConcurrentDictionary<PropertyInfo, PropertyConfigure> propertiesConfigures
            = new ConcurrentDictionary<PropertyInfo, PropertyConfigure>();

        public TypeConfigure(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.properties = new HashSet<PropertyInfo>(typeof(TClass).GetRuntimeProperties());

            var t = typeof(TClass);
            while (this.inheritedTypes.Add(t = t?.GetTypeInfo().BaseType)) { }
            this.inheritedTypes.Remove(null);

            // load configure
            if (typeof(TClass).GetTypeInfo().GetCustomAttribute<CommandClassAttribute>() is
                CommandClassAttribute classAttribute)
            {
                this.HasCommandClassAttribute = true;
            }
        }

        public Type Type { get; } = typeof(TClass);

        public bool HasCommandClassAttribute { get; }

        #region ITypeConfigure

        public ITypeConfigure GetInheritedTypeConfigure(Type declaringType)
        {
            if (declaringType == this.Type) return this;
            if (!this.inheritedTypes.Contains(declaringType)) throw new InvalidOperationException();

            if (this.inheritedsConfigures.TryGetValue(declaringType, out var c)) return c;
            var configure = (ITypeConfigure)
                this.serviceProvider.GetRequiredService(typeof(TypeConfigure<>).MakeGenericType(declaringType));
            return this.inheritedsConfigures.GetOrAdd(declaringType, configure);
        }

        public IPropertyConfigure GetConfigure(PropertyInfo property)
        {
            if (!this.properties.Contains(property)) throw new InvalidOperationException();

            if (this.propertiesConfigures.TryGetValue(property, out var r))
            {
                return r;
            }

            if (property.DeclaringType != this.Type)
            {
                return this.GetInheritedTypeConfigure(property.DeclaringType).GetConfigure(property);
            }
            else
            {
                return this.propertiesConfigures.GetOrAdd(property, (_) => new PropertyConfigure(this.serviceProvider, property));
            }
        }

        #endregion        

        internal class PropertyConfigure : IPropertyConfigure
        {
            private readonly IServiceProvider serviceProvider;

            public PropertyConfigure(IServiceProvider serviceProvider, PropertyInfo property)
            {
                this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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
            private readonly IServiceProvider serviceProvider;

            public ParameterAttribute(IServiceProvider serviceProvider, ParameterInfo parameter)
            {

            }
        }
    }
}
