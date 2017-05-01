using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Exceptions
{
    public class MapException : CliException
    {
        internal MapException([NotNull] Type type, string message)
            : base(message)
        {
            this.Type = type ?? throw new ArgumentNullException();
        }

        internal MapException([NotNull] Type type, [NotNull] MethodBase method, string message)
            : this(type, message)
        {
            this.Method = method ?? throw new ArgumentNullException();
        }

        public Type Type { get; }

        public MethodBase Method { get; }

        internal static MapException Create([NotNull] Type type, [NotNull] MethodBase method, string message)
        {
            var methodtype = method.Name.Replace("Info", "").ToLower();
            var typename = type.FullName;

            var sb = new StringBuilder();
            sb.AppendLine($"map {methodtype} of type <{typename}> error:");
            sb.Append("   ").AppendLine(message);
            sb.Append("   ").AppendLine("please contact developer.");
            return new MapException(type, method, sb.ToString());
        }
    }
}
