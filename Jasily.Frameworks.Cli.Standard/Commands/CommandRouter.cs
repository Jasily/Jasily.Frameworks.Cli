using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Core;
using Microsoft.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Jasily.Frameworks.Cli.IO;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Commands
{
    internal struct CommandRouter
    {
        private readonly IReadOnlyCollection<BindedCommand> _commands;
        private readonly IReadOnlyDictionary<string, BindedCommand> _commandsMap;

        private CommandRouter(StringComparer comparer, IEnumerable<BindedCommand> commands)
        {
            this._commands = commands.ToArray().AsReadOnly();
            var map = new Dictionary<string, BindedCommand>(comparer);
            foreach (var cmd in this._commands)
            {
                foreach (var name in cmd.Properties.Names)
                {
                    if (map.TryGetValue(name, out var x))
                    {
                        if (!x.Equals(cmd))
                        {
                            throw new InvalidOperationException($"Two commands map to same name <{name}>");
                        }
                    }
                    else
                    {
                        map.Add(name, cmd);
                    }
                    
                }
            }
            this._commandsMap = map;
            this.CommandsProperties = this._commands.Select(z => z.Properties).ToArray().AsReadOnly();
        }

        public IReadOnlyCollection<ICommandProperties> CommandsProperties { get; }

        public object Execute(IServiceProvider serviceProvider)
        {
            var session = (Session) serviceProvider.GetRequiredService<ISession>();
            session.AddRouter(this);
            var command = this.ResolveCommand(serviceProvider, session);
            session.AddCommand(command.InnerCommand);
            return command.Invoke(serviceProvider);
        }

        private BindedCommand ResolveCommand(IServiceProvider serviceProvider, ISession session)
        {
            var args = session.Argv;

            if (this._commands.Count == 0) return session.UnknownCommand<BindedCommand>();

            if (args.TryGetNextArgument(out var name))
            {
                if (this._commandsMap.TryGetValue(name, out var x))
                {
                    args.UseOne();
                    return x;
                }

                if (args.Argv.Count - args.UsedArgvCount == 1 &&
                    serviceProvider.GetRequiredService<HelpCommandsConfiguration>().Commands.Contains(name))
                {
                    // ignore.
                }
                else
                {
                    return session.UnknownCommand<BindedCommand>();
                }
            }

            session.DrawUsage();
            session.Termination();
            return default(BindedCommand);
        }

        internal static CommandRouter Build(IServiceProvider provider, object instance)
        {
            if (instance == null) throw new InvalidOperationException();
            var comparer = provider.GetRequiredService<StringComparer>();
            IEnumerable<BindedCommand> FindCommand()
            {
                var type = typeof(TypeConfiguration<>).FastMakeGenericType(instance.GetType());
                var configuration = (ITypeConfiguration)provider.GetRequiredService(type);
                return configuration.AvailableCommands.Select(z => new BindedCommand(z, instance));
            }
            return new CommandRouter(comparer, FindCommand());
        }
    }
}
