using System;
using System.Linq;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli.Converters
{
    internal class ArrayConverter<T> : IValueConverter<T[]>
    {
        private readonly IValueConverter<T> baseConverter;

        public ArrayConverter(IValueConverter<T> baseConverter)
        {
            if (typeof(T) == typeof(string))
            this.baseConverter = baseConverter;
        }

        public bool CanConvertFrom(object value)
        {
            return value is ArgumentValue val && val.Values.All(z => this.baseConverter.CanConvertFrom(z));
        }

        public T[] Convert(object value)
        {
            return ((ArgumentValue)value).Values.Select(z => this.baseConverter.Convert(z)).ToArray();
        }
    }
}
