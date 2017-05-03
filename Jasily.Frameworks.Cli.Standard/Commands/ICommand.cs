using System;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Commands
{
    internal interface ICommand
    {
        object Invoke(IServiceProvider serviceProvider, object instance);

        ICommandProperties Properties { get; }

        IReadOnlyList<IParameterConfiguration> ParameterConfigurations { get; }
    }
}