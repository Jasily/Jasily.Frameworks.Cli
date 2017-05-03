using System;
using System.Collections.Generic;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Configurations;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Commands
{
    /// <summary>
    /// provide binded instance command wrapper.
    /// </summary>
    internal class BindedCommand
    {
        private readonly object _instance;
        private readonly ICommand _innerCommand;

        public BindedCommand([NotNull] ICommand innerCommand, [NotNull] object instance)
        {
            this._innerCommand = innerCommand ?? throw new ArgumentNullException(nameof(innerCommand));
            this._instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public object Invoke(IServiceProvider serviceProvider)
        {
            return this._innerCommand.Invoke(serviceProvider, this._instance);
        }

        public ICommandProperties Properties => this._innerCommand.Properties;
    }
}
