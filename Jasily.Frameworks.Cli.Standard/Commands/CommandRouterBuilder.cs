using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Commands
{
    internal struct CommandRouterBuilder
    {
        private readonly object _instances;

        internal CommandRouterBuilder(object instances)
        {
            this._instances = instances ?? throw new ArgumentNullException(nameof(instances));
        }

        public CommandRouter Build(IServiceProvider provider)
        {
            if (this._instances == null) throw new InvalidOperationException();
            var comparer = provider.GetRequiredService<StringComparer>();
            var commands = new List<ICommand>(FindCommand(provider, this._instances));
            return new CommandRouter(comparer, commands);
        }

        private static IEnumerable<ICommand> FindCommand(IServiceProvider provider, object instance)
        {
            var type = typeof(TypeConfiguration<>).FastMakeGenericType(instance.GetType());
            var configuration = (ITypeConfiguration)provider.GetRequiredService(type);
            return configuration.AvailableCommands.Select(z => new InstancedCommand(z, instance));
        }
    }
}
