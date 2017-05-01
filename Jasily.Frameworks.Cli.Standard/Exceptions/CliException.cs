using System;

namespace Jasily.Frameworks.Cli.Exceptions
{
    /// <summary>
    /// base <see cref="Exception"/> for cli engine.
    /// if any <see cref="Exception"/> throw which Inherit by <see cref="CliException"/>, it will catch by <see cref="IEngine"/>.
    /// </summary>
    public class CliException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        internal CliException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        internal CliException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
