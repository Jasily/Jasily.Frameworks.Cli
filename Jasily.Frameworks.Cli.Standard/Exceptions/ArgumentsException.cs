using System;

namespace Jasily.Frameworks.Cli.Exceptions
{
    /// <summary>
    /// base <see cref="Exception"/> for user input error.
    /// </summary>
    public class ArgumentsException : CliException
    {
        internal ArgumentsException(string message)
            : base(message)
        {
        }

        internal ArgumentsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
