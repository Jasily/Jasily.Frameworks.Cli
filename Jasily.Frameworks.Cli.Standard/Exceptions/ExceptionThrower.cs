using System;
using System.Linq;
using System.Text;
using Jasily.Frameworks.Cli.Core;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Exceptions
{
    internal static class ExceptionThrower
    {
        /// <summary>
        ///  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <returns></returns>
        public static T UnknownArguments<T>([NotNull] this ISession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            var args = session.Argv.GetUnusedArguments();
            throw new ArgumentsException($"Unknown Arguments: ({string.Join(", ", args)})");
        }

        public static T UnknownCommand<T>([NotNull] this ISession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            var args = session.Argv.GetUnusedArguments();
            throw new ArgumentsException($"Unknown Command: <{args.First()}>");
        }

        internal static T InvalidArgument<T>([NotNull] this ArgumentValue value, string requireDetail)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var sb = new StringBuilder();
            sb.Append($"Parameter <{value.ParameterProperties.Names[0]}> Value Invalid Error: ");
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