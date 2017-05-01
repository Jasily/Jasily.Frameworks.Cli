using System;
using System.Linq;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli.Converters
{
    internal class ArrayConverter<T> : BaseConverter<T[]>
    {
        private readonly IValueConverter<T> baseConverter;

        public ArrayConverter(IValueConverter<T> baseConverter)
        {
            this.baseConverter = baseConverter;
        }

        public override T[] Convert(ArgumentValue value)
        {
            return value.Values.Select(z => this.baseConverter.Convert(z)).ToArray();
        }
    }
}
