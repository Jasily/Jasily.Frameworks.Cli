using System.Collections.Generic;
using System.Text;
using Jasily.Frameworks.Cli.Commands;
using System.Linq;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.IO
{
    internal class UsageDrawer : IUsageDrawer
    {
        private readonly IOutputer _outputer;

        public UsageDrawer(IOutputer outputer)
        {
            this._outputer = outputer;
        }

        public void DrawRouter(IReadOnlyCollection<ICommandProperties> commands)
        {
            this._outputer.WriteLine(OutputLevel.Usage, "Usage:");
            this._outputer.WriteLine(OutputLevel.Usage, "   CommandsProperties:");
            var sb = new StringBuilder();
            foreach (var cmd in commands)
            {
                sb.Clear();
                var names = new Queue<string>();
                foreach (var n in cmd.Names) names.Enqueue(n);                
                sb.Append("      ").Append(names.Dequeue());
                if (names.Count > 0)
                {
                    sb.Append("   ");
                    sb.Append($"(Alias: {string.Join(" / ", names)})");
                }                
                this._outputer.WriteLine(OutputLevel.Usage, sb.ToString());
            }
        }

        public void DrawParameter(ICommandProperties command, IReadOnlyList<IParameterProperties> parameters)
        {
            this._outputer.WriteLine(OutputLevel.Usage, "Usage:");
            this._outputer.WriteLine(OutputLevel.Usage, $"   Parameters of CommandsProperties <{command.Names[0]}>:");
            var sb = new StringBuilder();
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

                sb.Clear();
                sb.Append("      ");
                if (IsOptional())
                {
                    sb.Append("(optional)");
                }
                else
                {
                    sb.Append("(required)");
                }

                sb.Append("   ");
                sb.Append(parameter.Names[0]);
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
                this._outputer.WriteLine(OutputLevel.Usage, sb.ToString());
            }
        }
    }
}
