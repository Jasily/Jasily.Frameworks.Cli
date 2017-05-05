namespace Jasily.Frameworks.Cli.Exceptions
{
    internal class InvalidArgumentException : ArgumentsException
    {
        public InvalidArgumentException(string message) : base(message)
        {
        }
    }
}