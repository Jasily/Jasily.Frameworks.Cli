using System;

namespace Jasily.Frameworks.Cli.Exceptions
{
    /// <summary>
    /// base <see cref="Exception"/> for user input error.
    /// </summary>
    internal class ArgumentsException : CliException
    {
        internal ArgumentsException(string message)
            : base(message)
        {
        }
    }
}
