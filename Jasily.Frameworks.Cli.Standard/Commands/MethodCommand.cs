using System;
using System.Reflection;
using Jasily.DependencyInjection.MethodInvoker;
using Microsoft.Extensions.DependencyInjection;
using Jasily.DependencyInjection.MemberInjection;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Attributes;
using System.Linq;

namespace Jasily.Frameworks.Cli.Commands
{
    internal sealed class MethodCommand<T> : RealizedCommand<T>
    {
        private IInstanceMethodInvoker<T> invoker;

        public MethodCommand(IServiceProvider serviceProvider, MethodInfo method)
            : base(serviceProvider, method, method.GetCustomAttribute<CommandMethodAttribute>())
        {
            this.DeclaringName = method.Name;
        }

        public override string DeclaringName { get; }

        public override object Invoke(T instance, IServiceProvider serviceProvider, OverrideArguments args)
        {
            if (this.invoker == null)
            {
                this.invoker = serviceProvider
                    .GetRequiredService<IMethodInvokerFactory<T>>()
                    .GetInstanceMethodInvoker((MethodInfo)this.Method);
            }

            return this.invoker.Invoke(instance, serviceProvider, args);
        }
    }
}
