using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli.Exceptions
{
    internal sealed class ConvertException : CliException
    {
        internal ConvertException(string message)
            : base(message)
        {
            
        }

        internal ConvertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
