using System;
using System.Collections.Generic;
using System.Reflection;
using Jasily.Frameworks.Cli.Commands;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal interface ITypeConfiguration : ICommandConfiguration
    {
        IReadOnlyList<ICommand> AvailableCommands { get; }

        bool CanBeResult { get; }
    }
}
