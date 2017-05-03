using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class UnknownArgumentsException : ArgumentsException
    {
        internal UnknownArgumentsException(string message) : base(message)
        {
        }
    }
}
