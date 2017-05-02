using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli.Exceptions
{
    public sealed class ConvertException : CliException
    {
        public ConvertException(string message) : base(message)
        {
            
        }
    }
}
