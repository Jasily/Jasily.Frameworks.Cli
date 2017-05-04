using System.Collections.Generic;
using System.Text;
using Jasily.Frameworks.Cli.Commands;
using System.Linq;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.IO
{
    internal class UsageDrawer : IUsageDrawer
    {
        private const int IndentCell = 3;
        private readonly IOutputer _outputer;

        public UsageDrawer(IOutputer outputer)
        {
            this._outputer = outputer;
        }

        public void DrawRouter(IReadOnlyCollection<ICommandProperties> commands)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.Append(' ', IndentCell * 1).AppendLine("Commands:");

            foreach (var cmd in commands)
            {
                var ns = new List<string> { cmd[KnownPropertiesNames.DisplayName] };
                ns.AddRange(cmd.Names);
                var names = new Queue<string>(ns);               
                sb.Append(' ', IndentCell * 2).Append(names.Dequeue());
                if (names.Count > 0)
                {
                    sb.Append(' ', 3).Append($"(Alias: {string.Join(" / ", names)})");
                }

                var desc = cmd[KnownPropertiesNames.Description];
                if (desc.Length > 0)
                {
                    sb.AppendLine().Append(' ', IndentCell * 3).Append(desc);
                }
                sb.AppendLine();
            }

            this._outputer.WriteLine(OutputLevel.Usage, sb.ToString());
        }

        public void DrawParameter(ICommandProperties command, IReadOnlyList<IParameterProperties> parameters)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.Append(' ', IndentCell * 1).AppendLine($"Parameters of Commands <{command[KnownPropertiesNames.DisplayName]}>:");
            
            foreach (var parameter in parameters)
            {
                if (parameter.IsResolveByEngine) continue;

                bool IsOptional()
                {
                    if (parameter.IsArray && parameter.ArrayMinLength > 0)
                    {
                        return false;
                    }

                    return parameter.IsOptional;
                }
                
                // content
                sb.Append(' ', IndentCell * 2);
                sb.Append(IsOptional() ? "(optional)" : "(required)");

                sb.Append(' ', 3);
                sb.Append(parameter[KnownPropertiesNames.DisplayName]);
                sb.Append(" : ");
                if (parameter.IsArray)
                {
                    sb.Append('[');
                    // ReSharper disable once PossibleNullReferenceException
                    sb.Append(parameter.ArrayElementType.Name);
                    sb.Append(", ...]");

                    void AppendIfRangeValid(string value)
                    {
                        if (parameter.ArrayMinLength > 0 || parameter.ArrayMinLength > 0) sb.Append(value);
                    }

                    AppendIfRangeValid(" require(");
                    if (parameter.ArrayMinLength > 0)
                    {
                        sb.Append(parameter.ArrayMinLength.ToString()).Append("<");
                    }
                    AppendIfRangeValid("COUNT");
                    if (parameter.ArrayMaxLength > 0)
                    {
                        sb.Append("<").Append(parameter.ArrayMaxLength.ToString());
                    }
                    AppendIfRangeValid(")");
                }
                else
                {
                    sb.Append(parameter.ParameterInfo.ParameterType.Name);
                }
                if (parameter.Names.Count > 1)
                {
                    sb.Append("   ");
                    sb.Append($"(Alias: {string.Join(" / ", parameter.Names.Skip(1))})");
                }

                // desc
                var desc = parameter[KnownPropertiesNames.Description];
                if (desc.Length > 0)
                {
                    sb.AppendLine().Append(' ', IndentCell * 3).Append(desc);
                }

                // end
                sb.AppendLine();
            }

            this._outputer.WriteLine(OutputLevel.Usage, sb.ToString());
        }
    }
}
