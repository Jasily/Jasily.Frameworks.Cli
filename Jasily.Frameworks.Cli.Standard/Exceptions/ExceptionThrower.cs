namespace Jasily.Frameworks.Cli.Exceptions
{
    internal static class ExceptionThrower
    {
        public static T UnknownArguments<T>(this ISession session)
        {
            var args = session.Argv.GetUnusedArguments();
            var msg = $"unknown args: ({string.Join(", ", args)})";
            throw new UnknownArgumentsException(msg);
        }
    }
}