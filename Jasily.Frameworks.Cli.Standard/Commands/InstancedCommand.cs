using System;
using System.Collections.Generic;
using Jasily.DependencyInjection.MethodInvoker;

namespace Jasily.Frameworks.Cli.Commands
{
    internal class InstancedCommand : ICallableCommand
    {
        private readonly object instance;
        private readonly RealizedCommand innerCommand;

        public InstancedCommand(RealizedCommand innerCommand, object instance)
        {
            this.innerCommand = innerCommand;
            this.instance = instance;
        }

        public IReadOnlyList<string> Names => this.innerCommand.Names;

        public string DeclaringName => this.innerCommand.DeclaringName;

        public bool IgnoreDeclaringName => this.innerCommand.IgnoreDeclaringName;

        public object Invoke(IServiceProvider serviceProvider)
        {
            return this.innerCommand.Invoke(serviceProvider, this.instance);
        }
    }
}
