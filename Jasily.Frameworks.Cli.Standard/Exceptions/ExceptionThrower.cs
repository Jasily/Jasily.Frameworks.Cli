using System.Linq;

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
    }
}