using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Exceptions;
using Jasily.Frameworks.Cli.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Core
{
    internal class Session : ISession
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<CommandRouter> _routers = new List<CommandRouter>();
        private ICommand _command;

        public Session(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IReadOnlyList<string> OriginalArgv { get; set; }

        public IArgumentList Argv { get; set; }

        public void DrawUsage()
        {
            var drawer = this._serviceProvider.GetRequiredService<IUsageDrawer>();

            if (this._command != null)
            {
                drawer.DrawParameter(this._command.Properties, this._command.ParameterConfigurations);
            }
            else
            {
                drawer.DrawRouter(this._routers.Last());
            }
        }

        public void Termination()
        {
            throw new TerminationException();
        }

        public void Dispose()
        {
            
        }

        public void AddRouter(CommandRouter router)
        {
            this._command = null;
            this._routers.Add(router);
        }

        public void AddCommand(ICommand command)
        {
            this._command = command;
        }
    }
}
