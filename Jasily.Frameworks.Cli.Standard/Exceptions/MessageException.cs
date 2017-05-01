using System;

namespace Jasily.Frameworks.Cli.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException(string message)
            : base(message)
        {

        }
    }
}
