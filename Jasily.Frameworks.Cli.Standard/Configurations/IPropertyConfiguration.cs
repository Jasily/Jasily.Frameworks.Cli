using System;
using System.Reflection;
using Jasily.Frameworks.Cli.Commands;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal interface IPropertyConfiguration : ICommandConfiguration
    {
        PropertyInfo Property { get; }

        [CanBeNull]
        BaseCommand TryMakeCommand(Type type);
    }
}
