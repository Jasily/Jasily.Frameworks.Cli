using System;
using System.Linq;
using System.Text;
using Jasily.Frameworks.Cli.Core;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Exceptions
{
    internal static class ExceptionThrower
    {
        public static T UnknownArguments<T>(this ISession session)
        {
            var args = session.Argv.GetUnusedArguments();
            throw new UnknownArgumentsException($"Unknown Arguments: ({string.Join(", ", args)})");
        }

        public static T UnknownCommand<T>(this ISession session)
        {
            var args = session.Argv.GetUnusedArguments();
            throw new UnknownCommandException($"Unknown Command: <{args.First()}>");
        }

        internal static T InvalidArgument<T>([NotNull] this ArgumentValue value, string requireDetail)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var sb = new StringBuilder();
            sb.Append($"Parameter <{value.ParameterProperties.Names[0]}> Value Invalid Error: require ");
            sb.Append(requireDetail);
            throw new ArgumentsException(sb.ToString());
        }

        public static T UnResolveArgument<T>([NotNull] this ArgumentValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            throw new ArgumentsException($"Parameter <{value.ParameterName}> Error: Value is Missing.");
        }
    }
}