using System;
using System.Text;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Converters
{
    internal class EnumConverter<T> : BaseConverter<T> where T : struct
    {
        protected override T Convert(string value)
        {
            if (Enum.TryParse(value, out T val))
            {
                return val;
            }
            else
            {
                throw new ConvertException(new StringBuilder()
                    .AppendLine($"connot convert value <{value}> to type <{typeof(T).Name}>, valid value is:")
                    .AppendLine($"   {string.Join("|", Enum.GetNames(typeof(T)))}")
                    .ToString());
            }
        }
    }
}