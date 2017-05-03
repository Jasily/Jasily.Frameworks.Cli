using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jasily.DependencyInjection.AwaiterAdapter;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Core;
using Jasily.Frameworks.Cli.Exceptions;
using Jasily.Frameworks.Cli.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Commands
{
    internal abstract class BaseCommand : ICommand
    {
        protected BaseCommand(MethodBase method, ICommandProperties properties)
        {
            this.Method = method;
            this.Properties = properties;
        }

        public MethodBase Method { get; }

        public abstract IReadOnlyList<IParameterConfiguration> ParameterConfigurations { get; }

        public object Invoke(IServiceProvider serviceProvider, [NotNull] object instance)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            var session = serviceProvider.GetRequiredService<ISession>();

            OverrideArguments GetArguments()
            {
                try
                {
                    return this.ResolveArguments(serviceProvider, session.Argv);
                }
                catch (ArgumentsException e)
                {
                    serviceProvider.GetRequiredService<IOutputer>().WriteLine(OutputLevel.Error, e.Message);
                    session.DrawUsage();
                    session.Termination();
                    throw;
                }
            }

            var overrideArguments = GetArguments();

            // on interactive mode, arguments cannot 
            if (session.ExecuteMode == ExecuteMode.Interactive)
            {
                if (!session.Argv.IsAllUsed())
                {
                    return session.UnknownArguments<object>();
                }
            }

            object InternalInvoke()
            {
                try
                {
                    return this.Invoke(instance, serviceProvider, overrideArguments);
                }
                catch (ParameterResolveException)
                {
                    session.DrawUsage();
                    session.Termination();
                    throw;
                }
            }
            var value  = InternalInvoke();

            // wait for any kind task
            if (value != null)
            {
                value = serviceProvider.GetValueOrAwaitableResult(value, true);
            }

            object ContinueExecute()
            {
                session.Argv.Grouped();
                var router = CommandRouter.Build(serviceProvider, value);
                return router.Execute(serviceProvider);
            }

            if (value != null)
            {
                var type = typeof(TypeConfiguration<>).FastMakeGenericType(value.GetType());
                var configuration = (ITypeConfiguration) serviceProvider.GetRequiredService(type);

                if (!configuration.CanBeResult)
                {
                    return ContinueExecute();
                }

                if (!session.Argv.IsAllUsed() && configuration.IsDefinedCommand)
                {
                    return ContinueExecute();
                }
            }

            if (session.Argv.IsAllUsed())
            {
                return value;
            }

            return session.UnknownArguments<object>();
        }

        public ICommandProperties Properties { get; }

        public OverrideArguments ResolveArguments(IServiceProvider serviceProvider, IArgumentList args)
        {
            var oa = new OverrideArguments();
            if (this.ParameterConfigurations.Count > 0)
            {
                var avs = this.ParameterConfigurations
                    .Where(z => !z.IsResolveByEngine)
                    .Select(z => new ArgumentValue(z))
                    .ToArray()
                    .AsReadOnly();
                serviceProvider.GetRequiredService<IArgumentParser>().Parse(args, avs);
                foreach (var item in avs)
                {
                    oa.AddArgument(item.ParameterName, item.GetValue());
                }
            }
            return oa;
        }

        public abstract object Invoke(object instance, IServiceProvider serviceProvider, OverrideArguments args);
    }
}
