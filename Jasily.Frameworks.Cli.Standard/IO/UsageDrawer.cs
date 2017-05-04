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
        private readonly StringBuilder _sb = new StringBuilder();

        public UsageDrawer(IOutputer outputer)
        {
            this._outputer = outputer;
        }

        private void AppendDescription(IBaseProperties properties)
        {
            var desc = properties[KnownPropertiesNames.Description];

            if (desc.Length > 0)
            {
                this._sb.AppendLine().Append(' ', IndentCell * 10).Append(desc);
            }
        }

        public void DrawRouter(IReadOnlyCollection<ICommandProperties> commands)
        {
            this._sb.AppendLine("Usage:");
            this._sb.Append(' ', IndentCell * 1).AppendLine("Commands:");

            foreach (var cmd in commands)
            {
                Queue<string> GetName()
                {
                    var ns = new List<string> { cmd[KnownPropertiesNames.DisplayName] };
                    ns.AddRange(cmd.Names);
                    return new Queue<string>(ns);
                }

                var names = GetName();
                var fn = names.Dequeue();
                this._sb.Append(' ', IndentCell * 2).Append(fn);
                if (names.Count > 0)
                {
                    if (fn.Length < IndentCell * 6)
                    {
                        this._sb.Append(' ', IndentCell * 7 - fn.Length);
                    }

                    this._sb.Append(' ', 3).Append($"(Alias: {string.Join(" / ", names)})");
                }
                this.AppendDescription(cmd);
                this._sb.AppendLine();
            }

            this._outputer.WriteLine(OutputLevel.Usage, this._sb.ToString());
        }

        public void DrawParameter(ICommandProperties command, IReadOnlyList<IParameterProperties> parameters)
        {
            this._sb.AppendLine("Usage:");
            this._sb.Append(' ', IndentCell * 1).AppendLine($"Parameters of Commands <{command[KnownPropertiesNames.DisplayName]}>:");
            
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
                this._sb.Append(' ', IndentCell * 2);
                this._sb.Append(IsOptional() ? "(optional)" : "(required)");

                this._sb.Append(' ');
                this._sb.Append(parameter[KnownPropertiesNames.DisplayName]);
                this._sb.Append(" : ");

                if (parameter.IsArray)
                {
                    this._sb.Append('[');
                    // ReSharper disable once PossibleNullReferenceException
                    this._sb.Append(parameter.ArrayElementType.Name);
                    this._sb.Append(", ...]");

                    void AppendIfRangeValid(string value)
                    {
                        if (parameter.ArrayMinLength > 0 || parameter.ArrayMinLength > 0) this._sb.Append(value);
                    }

                    AppendIfRangeValid(" require(");
                    if (parameter.ArrayMinLength > 0)
                    {
                        this._sb.Append(parameter.ArrayMinLength.ToString()).Append("<");
                    }
                    AppendIfRangeValid("COUNT");
                    if (parameter.ArrayMaxLength > 0)
                    {
                        this._sb.Append("<").Append(parameter.ArrayMaxLength.ToString());
                    }
                    AppendIfRangeValid(")");
                }
                else
                {
                    this._sb.Append(parameter.ParameterInfo.ParameterType.Name);
                }

                if (parameter.Names.Count > 1)
                {
                    this._sb.Append("   ");
                    this._sb.Append($"(Alias: {string.Join(" / ", parameter.Names.Skip(1))})");
                }

                // desc
                this.AppendDescription(parameter);

                // end
                this._sb.AppendLine();
            }

            this._outputer.WriteLine(OutputLevel.Usage, this._sb.ToString());
        }
    }
}
