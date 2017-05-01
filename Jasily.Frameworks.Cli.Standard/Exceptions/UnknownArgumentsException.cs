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

        internal static UnknownArgumentsException Build(ISession session)
        {
            var args = session.Argv.GetUnusedArguments();
            var msg = $"unknown args: ({string.Join(", ", args)})";
            return new UnknownArgumentsException(msg);
        }
    }
}
