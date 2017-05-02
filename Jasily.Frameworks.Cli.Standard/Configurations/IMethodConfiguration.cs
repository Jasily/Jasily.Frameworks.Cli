using System;
using System.Collections.Generic;
using System.Reflection;
using Jasily.Frameworks.Cli.Commands;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal interface IMethodConfiguration : ICommandConfiguration
    {
        [NotNull]
        MethodInfo Method { get; }

        [CanBeNull]
        BaseCommand TryMakeCommand(Type type);

        [NotNull]
        IReadOnlyList<IParameterConfiguration> ParameterConfigurations { get; }
    }
}