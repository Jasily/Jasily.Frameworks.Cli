using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Jasily.Frameworks.Cli.IO;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Commands
{
    internal class CommandRouter : ICommandRouter
    {
        public IReadOnlyCollection<ICallableCommand> Commands { get; }

        public IReadOnlyDictionary<string, ICallableCommand> CommandsMap { get; }

        IReadOnlyCollection<ICommandProperties> ICommandRouter.Commands => this.Commands;

        public CommandRouter(StringComparer comparer, IEnumerable<ICallableCommand> commands)
        {
            this.Commands = commands.ToArray();
            var map = new Dictionary<string, ICallableCommand>(comparer);
            foreach (var cmd in commands)
            {
                var mapnames = cmd.IgnoreDeclaringName ? cmd.Names : cmd.Names.Concat(new string[] { cmd.DeclaringName });
                foreach (var name in mapnames)
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

            bool TryResolve(IArgumentList args, out ICallableCommand command)
            {
                switch (this.Commands.Count)
                {
                    case 0:
                        command = default(ICallableCommand);
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
                        command = default(ICallableCommand);
                        return false;
                }
            }
                        
            if (TryResolve(session.Argv, out var cmd))
            {
                return cmd.Invoke(serviceProvider);
            }
            else
            {
                serviceProvider.GetRequiredService<IUsageDrawer>().DrawRouter(this);
                throw new TerminationException();
            }
        }
    }
}
