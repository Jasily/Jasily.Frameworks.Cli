using System;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Commands;

namespace Jasily.Frameworks.Cli.Core
{
    internal class Engine : IEngine
    {
        public IServiceProvider ServiceProvider { get; }

        public Engine(IServiceProvider serviceServiceProvider)
        {
            this.ServiceProvider = serviceServiceProvider;
        }

        public Executor Fire(object instance)
        {
            return new Executor(this, instance);
        }
    }
}
