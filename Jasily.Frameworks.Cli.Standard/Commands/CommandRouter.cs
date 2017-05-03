using System;
using System.Linq;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Core;
using Microsoft.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Jasily.Frameworks.Cli.IO;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Commands
{
    internal class CommandRouter : ICommandRouter
    {
        public IReadOnlyCollection<ICommand> Commands { get; }

        public IReadOnlyDictionary<string, ICommand> CommandsMap { get; }

        IReadOnlyCollection<ICommandProperties> ICommandRouter.Commands => this.Commands.Select(z => z.Properties).ToArray()
            .AsReadOnly();

        public CommandRouter(StringComparer comparer, IEnumerable<ICommand> commands)
        {
            this.Commands = commands.ToArray().AsReadOnly();
            var map = new Dictionary<string, ICommand>(comparer);
            foreach (var cmd in this.Commands)
            {
                foreach (var name in cmd.Properties.Names)
                {
                    if (map.TryGetValue(name, out var x) && x != cmd)
                    {
                        if (x != cmd) throw new InvalidOperationException();
                    }
                    else
                    {
                        map.Add(name, cmd);
                    }
                    
                }
            }
            this.CommandsMap = map;
        }

        public object Execute(IServiceProvider serviceProvider)
        {
            var session = (Session) serviceProvider.GetRequiredService<ISession>();
            session.AddRouter(this);
            return this.ResolveCommand(serviceProvider, session).Invoke(serviceProvider, null);
        }

        private ICommand ResolveCommand(IServiceProvider serviceProvider, ISession session)
        {
            var args = session.Argv;

            if (this.Commands.Count == 0) return session.UnknownArguments<ICommand>();

            if (args.TryGetNextArgument(out var name))
            {
                if (this.CommandsMap.TryGetValue(name, out var command))
                {
                    args.UseOne();
                    return command;
                }

                if (args.Argv.Count - args.UsedArgvCount == 1 &&
                    serviceProvider.GetRequiredService<HelpCommandsConfiguration>().Commands.Contains(name))
                {
                    // ignore.
                }
                else
                {
                    return session.UnknownCommand<ICommand>();
                }
            }
            session.DrawUsage();
            session.Termination();
            return null;
        }
    }
}
