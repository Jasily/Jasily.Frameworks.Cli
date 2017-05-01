using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Jasily.Frameworks.Cli.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Commands
{
    internal struct CommandRouterBuilder
    {
        private HashSet<Type> types;
        private List<object> instances;

        public CommandRouterBuilder(params object[] instances)
        {
            if (instances == null) throw new ArgumentNullException(nameof(instances));
            if (instances.Any(z => z == null)) throw new ArgumentException();

            this.instances = instances.ToList();
            this.types = new HashSet<Type>(instances.Select(z => z.GetType()));
            if (this.instances.Count != this.types.Count) throw new InvalidOperationException();
        }

        public void Add(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (!this.types.Add(instance.GetType())) throw new InvalidOperationException();

            this.instances.Add(instance);
        }

        public CommandRouter Build(IServiceProvider provider)
        {
            var comparer = provider.GetRequiredService<StringComparer>();

            if ((this.instances?.Count ?? 0) == 0)
            {
                return new CommandRouter(comparer, Enumerable.Empty<ICallableCommand>());
            }

            var commands = new List<ICallableCommand>(this.instances.Count == 1
                ? this.FindCommand(provider, this.instances[0])
                : this.FindCommand(provider, this.instances));

            return new CommandRouter(comparer, commands);
        }

        private IEnumerable<ICallableCommand> FindCommand(IServiceProvider provider, object instance)
        {
            var type = typeof(ClassCommand<>).MakeGenericType(instance.GetType());
            var classCommand = (IClassCommand)provider.GetRequiredService(type);
            return classCommand.SubCommands.Select(z => new InstancedCommand(z, instance));
        }

        private IEnumerable<ICallableCommand> FindCommand(IServiceProvider provider, IEnumerable<object> instances)
        {
            return instances
                .Select(obj => {
                    var type = typeof(ClassCommand<>).MakeGenericType(obj.GetType());
                    var cmd = (RealizedCommand)provider.GetRequiredService(type);
                    return new InstancedCommand(cmd, obj);
                });
        }
    }
}
