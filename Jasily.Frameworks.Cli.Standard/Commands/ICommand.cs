using System;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Commands
{
    internal interface ICommand
    {
        object Invoke(IServiceProvider serviceProvider, object instance);

        ICommandProperties Properties { get; }
    }
}