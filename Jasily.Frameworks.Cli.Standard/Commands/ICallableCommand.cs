using System;

namespace Jasily.Frameworks.Cli.Commands
{
    internal interface ICallableCommand : ICommandProperties
    {
        object Invoke(IServiceProvider serviceProvider);
    }
}
