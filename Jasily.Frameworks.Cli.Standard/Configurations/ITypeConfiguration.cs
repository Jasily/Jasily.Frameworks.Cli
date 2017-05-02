using System;
using System.Collections.Generic;
using System.Reflection;
using Jasily.Frameworks.Cli.Commands;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal interface ITypeConfiguration : ICommandConfiguration
    {
        IPropertyConfiguration GetConfigure(PropertyInfo property);

        IReadOnlyList<ICommand> AvailableCommands { get; }
    }
}
