using System;
using System.Collections.Generic;
using System.Reflection;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Commands
{
    internal sealed class PropertyCommand<T> : BaseCommand
    {
        private IInstanceMethodInvoker<T> _invoker;

        public PropertyCommand(TypeConfiguration<T>.PropertyConfiguration configuration) : base(
            configuration.ServiceProvider, configuration.Property.GetMethod, configuration)
        {
            
        }

        public override object Invoke(object instance, IServiceProvider serviceProvider, OverrideArguments args)
        {
            if (this._invoker == null)
            {
                this._invoker = serviceProvider
                    .GetRequiredService<IMethodInvokerFactory<T>>()
                    .GetInstanceMethodInvoker((MethodInfo)this.Method);
            }

            return this._invoker.Invoke((T)instance, serviceProvider, args);
        }
    }
}
