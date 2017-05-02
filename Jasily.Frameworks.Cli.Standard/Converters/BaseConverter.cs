using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Exceptions;
using Jasily.Frameworks.Cli.Core;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Converters
{
    public abstract class BaseConverter<T> : IValueConverter<T>
    {
        public object Convert([NotNull] IEnumerable<string> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            return values.Select(this.Convert).ToArray();
        }

        object IValueConverter.Convert(string value)
        {
            try
            {
                return this.Convert(value);
            }
            catch (ConvertException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ConvertException($"connot convert value <{value}> to type <{typeof(T).Name}>");
            }
        }

        protected abstract T Convert([NotNull] string value);
    }
}
