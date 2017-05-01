using System;
using System.Collections.Generic;
using System.Text;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli.Converters
{
    public abstract class BaseConverter<T> : IValueConverter<T>
    {
        public bool CanConvertFrom(object value)
        {
            return value is ArgumentValue;
        }

        public T Convert(object value)
        {
            return this.Convert((ArgumentValue)value);
        }

        public abstract T Convert(ArgumentValue value);
    }
}
