using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli.Exceptions
{
    public class ConvertException : Exception
    {
        public ConvertException(ArgumentValue argument, string message)
            : base(message)
        {
            this.Argument = argument;
        }

        public ConvertException(ArgumentValue argument, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Argument = argument;
        }

        public ArgumentValue Argument { get; }
    }
}
