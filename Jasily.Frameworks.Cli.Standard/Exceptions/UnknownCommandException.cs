namespace Jasily.Frameworks.Cli.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class UnknownCommandException : ArgumentsException
    {
        internal UnknownCommandException(string message) : base(message)
        {
        }
    }
}