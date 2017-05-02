using System;
using System.Reflection;
using Jasily.Frameworks.Cli.Commands;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal interface IMethodConfiguration : ICommandConfiguration
    {
        MethodInfo Method { get; }

        BaseCommand TryMakeCommand(Type type);
    }
}