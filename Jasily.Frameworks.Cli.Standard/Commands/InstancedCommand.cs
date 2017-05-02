using System;
using System.Collections.Generic;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Configurations;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Commands
{
    internal class InstancedCommand : ICommand
    {
        private readonly object _instance;
        private readonly ICommand _innerCommand;

        public InstancedCommand([NotNull] ICommand innerCommand, [NotNull] object instance)
        {
            this._innerCommand = innerCommand ?? throw new ArgumentNullException(nameof(innerCommand));
            this._instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public object Invoke(IServiceProvider serviceProvider, object instance)
        {
            if (instance != null) throw new ArgumentException("WTF");
            return this._innerCommand.Invoke(serviceProvider, this._instance);
        }

        public ICommandProperties Properties => this._innerCommand.Properties;
    }
}
