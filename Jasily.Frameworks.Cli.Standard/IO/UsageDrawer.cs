using System.Collections.Generic;
using System.Text;
using Jasily.Frameworks.Cli.Commands;
using System.Linq;

namespace Jasily.Frameworks.Cli.IO
{
    internal class UsageDrawer : IUsageDrawer
    {
        private readonly IOutputer outputer;

        public UsageDrawer(IOutputer outputer)
        {
            this.outputer = outputer;
        }

        public void DrawRouter(ICommandRouter router)
        {
            this.outputer.WriteLine(OutputLevel.Usage, "Usage:");
            this.outputer.WriteLine(OutputLevel.Usage, "   Commands:");
            var sb = new StringBuilder();
            foreach (var cmd in router.Commands)
            {
                sb.Clear();
                var names = new Queue<string>();
                if (!cmd.IgnoreDeclaringName) names.Enqueue(cmd.DeclaringName);
                foreach (var n in cmd.Names) names.Enqueue(n);                
                sb.Append("      ").Append(names.Dequeue());
                if (names.Count > 0)
                {
                    sb.Append("   ");
                    sb.Append($"(Alias: {string.Join(" / ", names)})");
                }                
                this.outputer.WriteLine(OutputLevel.Usage, sb.ToString());
            }
        }

        public void DrawParameter(ICommandProperties cmd, IReadOnlyList<ParameterInfoDescriptor> parameters)
        {
            this.outputer.WriteLine(OutputLevel.Usage, "Usage:");
            var cmdName = cmd.IgnoreDeclaringName ? cmd.Names[0] : cmd.DeclaringName;
            this.outputer.WriteLine(OutputLevel.Usage, $"   Parameters of Commands <{cmdName}>:");
            var sb = new StringBuilder();
            foreach (var parameter in parameters)
            {
                if (parameter.IsAutoPadding) continue;

                bool IsOptional()
                {
                    if (parameter.IsArray && parameter.ArrayMinLength is uint val)
                    {
                        return val == 0;
                    }

                    return parameter.ParameterInfo.HasDefaultValue;
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
                var name = parameter.ParameterInfo.Name;
                sb.Append("   ");
                sb.Append(name);
                sb.Append(" : ");
                if (parameter.IsArray)
                {
                    sb.Append('[');
                    sb.Append(parameter.ParameterInfo.ParameterType.GetElementType().Name);
                    sb.Append(", ...]");
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
                this.outputer.WriteLine(OutputLevel.Usage, sb.ToString());
            }
        }
    }
}
