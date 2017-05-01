using System;
using System.IO;
using System.Linq;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Exceptions;
using Jasily.Frameworks.Cli.Core;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Converters
{
    public abstract class StringConverter<T> : BaseConverter<T>
    {
        public abstract T Convert([NotNull] string value);

        public override T Convert([NotNull] ArgumentValue value)
        {
            if (value.Values.Count > 1)
            {
                throw new ConvertException(value, "too many arguments.");
            }

            var str = value.Values.Single();
            try
            {
                return this.Convert(str);
            } 
            catch (Exception e)
            {
                throw new ConvertException(value, e.Message, e);
            }
        }
    }

    public class StringConverter : StringConverter<string>
    {
        public override string Convert([NotNull] string value)
        {
            return value;
        }
    }
}
