using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UnknownArgumentsException : ArgumentsException
    {
        internal UnknownArgumentsException(string message) : base(message)
        {
        }
    }
}
