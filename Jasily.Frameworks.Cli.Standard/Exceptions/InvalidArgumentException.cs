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
            sb.AppendLine($"invalid value in parameter <{value.Parameter.ParameterInfo.Name}>:");
            sb.AppendLine("require:");
            sb.AppendLine(require);
            return new InvalidArgumentException(require);
        }
    }
}
