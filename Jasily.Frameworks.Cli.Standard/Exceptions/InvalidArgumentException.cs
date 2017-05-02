using System.Text;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidArgumentException : ArgumentsException
    {
        internal InvalidArgumentException(string message) : base(message)
        {

        }

        internal static InvalidArgumentException Build(ArgumentValue value, string require)
        {
            var sb = new StringBuilder();
            sb.Append($"invalid value for parameter <{value.ParameterProperties.Names[0]}>: require  ");
            sb.Append(require);
            return new InvalidArgumentException(sb.ToString());
        }
    }
}
