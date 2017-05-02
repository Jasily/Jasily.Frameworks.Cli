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
        private readonly HashSet<Type> _types;
        private readonly List<object> _instances;

        public CommandRouterBuilder(params object[] instances)
        {
            if (instances == null) throw new ArgumentNullException(nameof(instances));
            if (instances.Any(z => z == null)) throw new ArgumentException();

            this._instances = instances.ToList();
            this._types = new HashSet<Type>(instances.Select(z => z.GetType()));
            if (this._instances.Count != this._types.Count) throw new InvalidOperationException();
        }

        public void Add(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (!this._types.Add(instance.GetType())) throw new InvalidOperationException();

            this._instances.Add(instance);
        }

        public CommandRouter Build(IServiceProvider provider)
        {
            var comparer = provider.GetRequiredService<StringComparer>();

            if (this._instances == null || this._instances.Count == 0)
            {
                return new CommandRouter(comparer, Enumerable.Empty<ICommand>());
            }

            var commands = new List<ICommand>(this._instances.Count == 1
                ? this.FindCommand(provider, this._instances[0])
                : this.FindCommand(provider, this._instances));

            return new CommandRouter(comparer, commands);
        }

        private IEnumerable<ICommand> FindCommand(IServiceProvider provider, object instance)
        {
            var type = typeof(TypeConfiguration<>).FastMakeGenericType(instance.GetType());
            var configuration = (ITypeConfiguration)provider.GetRequiredService(type);
            return configuration.AvailableCommands.Select(z => new InstancedCommand(z, instance));
        }

        private IEnumerable<ICommand> FindCommand(IServiceProvider provider, IEnumerable<object> instances)
        {
            return instances
                .Select(obj => {
                    var type = typeof(ClassCommand<>).MakeGenericType(obj.GetType());
                    var cmd = (BaseCommand)provider.GetRequiredService(type);
                    return new InstancedCommand(cmd, obj);
                });
        }
    }
}
