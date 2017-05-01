using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli.Exceptions
{
    public class UnknownArgumentsException : CliException
    {
        internal UnknownArgumentsException(ISession session, string[] args)
            : base($"unknown args: ({string.Join(", ", args)})")
        {
            this.Session = session ?? throw new ArgumentNullException();
            this.UnknownArguments = args ?? throw new ArgumentNullException();
        }

        public ISession Session { get; }

        public string[] UnknownArguments { get; }
    }
}
