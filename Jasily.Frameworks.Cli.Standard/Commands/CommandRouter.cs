using System;
using System.Linq;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Configurations;
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
            var session = serviceProvider.GetRequiredService<ISession>();

            bool TryResolve(IArgumentList args, out ICommand command)
            {
                switch (this.Commands.Count)
                {
                    case 0:
                        command = default(ICommand);
                        return session.UnknownArguments<bool>();

                    case 1:
                        command = this.Commands.Single();
                        return true;

                    default:
                        if (args.TryGetNextArgument(out var name))
                        {
                            if (this.CommandsMap.TryGetValue(name, out command))
                            {
                                args.UseOne();
                                return true;
                            }

                            serviceProvider.GetRequiredService<IOutputer>()
                                .WriteLine(OutputLevel.Error, $"unknown command <{name}>");
                        }
                        command = default(ICommand);
                        return false;
                }
            }
                        
            if (TryResolve(session.Argv, out var cmd))
            {
                return cmd.Invoke(serviceProvider, null);
            }
            else
            {
                serviceProvider.GetRequiredService<IUsageDrawer>().DrawRouter(this);
                throw new TerminationException();
            }
        }
    }
}
