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
    internal struct BindedCommand : IEquatable<BindedCommand>
    {
        public BindedCommand([NotNull] ICommand innerCommand, [NotNull] object instance)
        {
            this.InnerCommand = innerCommand ?? throw new ArgumentNullException(nameof(innerCommand));
            this.Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public object Instance { get; }

        public object Invoke(IServiceProvider serviceProvider)
        {
            return this.InnerCommand.Invoke(serviceProvider, this.Instance);
        }

        public bool Equals(BindedCommand other) => this.InnerCommand == other.InnerCommand;

        public ICommand InnerCommand { get; }

        public ICommandProperties Properties => this.InnerCommand.Properties;
    }
}
