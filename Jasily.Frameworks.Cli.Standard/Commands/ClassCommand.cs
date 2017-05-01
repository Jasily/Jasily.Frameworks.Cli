using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Commands
{
    internal interface IClassCommand
    {
        IReadOnlyList<RealizedCommand> SubCommands { get; }
    }

    internal class ClassCommand<T> : RealizedCommand, IClassCommand
    {
        private IReadOnlyList<RealizedCommand<T>> subCommands;
        private readonly IServiceProvider serviceProvider;

        public ClassCommand(IServiceProvider provider)
        {
            this.serviceProvider = provider;
            
            var type = typeof(T);
            this.AddName(type.Name);
            this.AddName(type.GetTypeInfo().GetCustomAttribute<CommandClassAttribute>()?.Names);
        }

        private static ConstructorInfo GetConstructor()
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var constructors = typeInfo.DeclaredConstructors
                .Where(z => z.GetCustomAttribute<CommandConstructorAttribute>() != null)
                .ToArray();
            if (constructors.Length > 1)
            {
                throw MapException.Create(typeof(T), constructors[1], $"contains too many <{nameof(CommandConstructorAttribute)}> attribute.");
            }

            if (constructors.Length == 0)
            {
                constructors = typeInfo.DeclaredConstructors.ToArray();
            }

            switch (constructors.Length)
            {
                case 0:
                    throw new NotImplementedException();
                    break;

                case 1:
                    return constructors[0];
                    break;

                default:
                    throw MapException.Create(typeof(T), constructors[1], $"contains too many public constructors.");
            }
        }

        public IReadOnlyList<RealizedCommand> SubCommands
        {
            get
            {
                if (this.subCommands == null)
                {
                    var cmds = new List<RealizedCommand<T>>();
                    var type = typeof(T);

                    foreach (var method in type.GetRuntimeMethods())
                    {
                        if (!method.IsPublic) continue;

                        var mt = typeof(MethodCommand<>).MakeGenericType(type);
                        var it = typeof(IMethodInvokerFactory<>).MakeGenericType(mt);
                        var oa = new OverrideArguments();
                        oa.AddArgument("method", method);
                        var factory = (IMethodInvokerFactory)this.serviceProvider.GetRequiredService(it);
                        var invoker = factory.GetConstructorInvoker(mt.GetTypeInfo().DeclaredConstructors.Single());
                        cmds.Add((RealizedCommand<T>)invoker.Invoke(this.serviceProvider, oa));
                    }

                    foreach (var property in type.GetRuntimeProperties())
                    {
                        if (!property.CanRead ||
                            !property.GetMethod.IsPublic ||
                            property.GetIndexParameters().Length > 0) continue;

                        var ct = typeof(PropertyCommand<>).MakeGenericType(type);
                        var it = typeof(IMethodInvokerFactory<>).MakeGenericType(ct);
                        var oa = new OverrideArguments();
                        oa.AddArgument("property", property);
                        var factory = (IMethodInvokerFactory) this.serviceProvider.GetRequiredService(it);
                        var invoker = factory.GetConstructorInvoker(ct.GetTypeInfo().DeclaredConstructors.Single());
                        cmds.Add((RealizedCommand<T>) invoker.Invoke(this.serviceProvider, oa));
                    }

                    Interlocked.CompareExchange(ref this.subCommands, new ReadOnlyCollection<RealizedCommand<T>>(cmds), null);
                }
                return this.subCommands;
            }
        }

        public override object Invoke(object instance, IServiceProvider serviceProvider, OverrideArguments args)
        {
            throw new NotImplementedException();
        }

        public override OverrideArguments ResolveArguments(IServiceProvider serviceProvider, IArgumentList args)
        {
            throw new NotImplementedException();
        }
    }
}
