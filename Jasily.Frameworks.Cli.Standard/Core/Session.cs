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
        private readonly List<ResolvedNode> _nodes = new List<ResolvedNode>();

        public Session(IServiceProvider serviceProvider, IArgumentList argv, SessionConfigurator configurator)
        {
            this._serviceProvider = serviceProvider;
            this.Argv = argv;
            this.ExecuteMode = configurator.Mode;
        }

        public ExecuteMode ExecuteMode { get; }

        public IArgumentList Argv { get; }

        public void DrawUsage()
        {
            var drawer = this._serviceProvider.GetRequiredService<IUsageDrawer>();
            this._nodes.Last().Draw(drawer);
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
            this._nodes.Add(new ResolvedNode(router));
        }

        public void AddCommand(ICommand command)
        {
            this._nodes.Add(new ResolvedNode(command));
        }

        private struct ResolvedNode
        {
            public ResolvedNode(CommandRouter router)
            {
                this.IsRouter = true;
                this.Router = router;
                this.Command = null;
            }

            public ResolvedNode(ICommand command)
            {
                this.IsRouter = false;
                this.Router = default(CommandRouter);
                this.Command = command;
            }

            public bool IsRouter { get; }

            public CommandRouter Router { get; }

            public ICommand Command { get; }

            public void Draw(IUsageDrawer drawer)
            {
                if (this.IsRouter)
                {
                    drawer.DrawRouter(this.Router.CommandsProperties);
                }
                else
                {
                    drawer.DrawParameter(this.Command.Properties, this.Command.ParameterConfigurations);
                }
            }
        }
    }
}
