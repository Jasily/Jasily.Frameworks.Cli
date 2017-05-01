using System;
using System.Collections.Generic;
using Jasily.DependencyInjection.MethodInvoker;

namespace Jasily.Frameworks.Cli.Commands
{
    internal class InstancedCommand : ICallableCommand
    {
        private readonly object instance;
        private readonly RealizedCommand implCommand;

        public InstancedCommand(RealizedCommand implCommand, object instance)
        {
            this.implCommand = implCommand;
            this.instance = instance;
        }

        public IEnumerable<string> EnumerateNames()
        {
            return this.implCommand.EnumerateNames();
        }

        public object Invoke(IServiceProvider serviceProvider)
        {
            return this.implCommand.Invoke(serviceProvider, this.instance);
        }
    }
}
