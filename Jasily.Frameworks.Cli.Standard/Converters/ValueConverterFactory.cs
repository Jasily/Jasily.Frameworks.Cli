using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Jasily.DependencyInjection.MethodInvoker;

namespace Jasily.Frameworks.Cli.Converters
{
    internal class ValueConverterFactory : IValueConverterFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ValueConverterFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IValueConverter<T> GetValueConverter<T>()
        {
            var type = this.ResolveConverterType(typeof(T));
            if (type != null)
            {
                var service = this.serviceProvider.GetService(type);
                if (service != null) return (IValueConverter<T>)service;
            }
            return null;
        }

        private Type ResolveConverterType(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (type.IsArray && this.serviceProvider.GetService(typeof(IValueConverter<>).MakeGenericType(type.GetElementType())) != null)
            {
                var eltype = type.GetElementType();
                if (this.serviceProvider.GetService(typeof(IValueConverter<>).MakeGenericType(eltype)) != null)
                {
                    return typeof(ArrayConverter<>).MakeGenericType(eltype);
                }
            }

            return null;
        }
    }
}
