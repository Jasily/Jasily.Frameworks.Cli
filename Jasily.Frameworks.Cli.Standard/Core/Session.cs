using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Exceptions;
using Jasily.Frameworks.Cli.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Core
{
    internal class Session : ISession
    {
        private readonly IServiceProvider _serviceProvider;
        private CommandRouter _router;
        private BaseCommand _command;

        public Session(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IReadOnlyList<string> OriginalArgv { get; set; }

        public IArgumentList Argv { get; set; }

        public void DrawUsage()
        {
            if (this._router == null && this._command == null)
            {
                throw new InvalidOperationException();
            }
            if (this._command != null)
            {
                this._serviceProvider.GetRequiredService<IUsageDrawer>()
                    .DrawParameter(this._command.Properties, this._command.ParameterConfigurations);
            }
            else if (this._router != null)
            {
                this._serviceProvider.GetRequiredService<IUsageDrawer>()
                    .DrawRouter(this._router);
            }
            else
            {
                throw new InvalidOperationException();
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
            this._router = router;
        }

        public void AddCommand(BaseCommand command)
        {
            this._router = null;
            this._command = command;
        }
    }
}
