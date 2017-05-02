using System;
using System.Reflection;
using Jasily.DependencyInjection.MethodInvoker;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Attributes;
using System.Linq;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Commands
{
    internal sealed class MethodCommand<T> : BaseCommand
    {
        private readonly TypeConfiguration<T>.MethodConfiguration _configuration;
        private IInstanceMethodInvoker<T> _invoker;

        public MethodCommand(TypeConfiguration<T>.MethodConfiguration configuration)
            : base(configuration.Method, configuration)
        {
            this._configuration = configuration;
        }

        public override IReadOnlyList<IParameterConfiguration> ParameterConfigurations => this._configuration.ParameterConfigurations;

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
