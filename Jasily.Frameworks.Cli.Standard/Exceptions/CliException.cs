using System;

namespace Jasily.Frameworks.Cli.Exceptions
{
    public class CliException : Exception
    {
        public CliException(string message) : base(message)
        {
        }

        public CliException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
