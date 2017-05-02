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
        protected BaseCommand(IServiceProvider serviceProvider, MethodBase method, ICommandProperties properties)
        {
            this.Method = method;
            this.Properties = properties;

            var comaprer = serviceProvider.GetRequiredService<StringComparer>();
            this.Parameters = method
                .GetParameters()
                .Select(z => new ParameterInfoDescriptor(z, comaprer))
                .ToArray();
        }

        public MethodBase Method { get; }

        public IReadOnlyList<ParameterInfoDescriptor> Parameters { get; }

        public object Invoke(IServiceProvider serviceProvider, [NotNull] object instance)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            var session = serviceProvider.GetRequiredService<ISession>();

            void DrawUsage()
            {
                serviceProvider.GetRequiredService<IUsageDrawer>()
                    .DrawParameter(this.Properties, this.Parameters);
                throw new TerminationException();
            }

            OverrideArguments GetArguments()
            {
                try
                {
                    return this.ResolveArguments(serviceProvider, session.Argv);
                }
                catch (ArgumentsException e)
                {
                    serviceProvider.GetRequiredService<IOutputer>().WriteLine(OutputLevel.Error, e.Message);
                    DrawUsage();
                    throw;
                }
            }
            var oa = GetArguments();

            object InternalInvoke()
            {
                try
                {
                    return this.Invoke(instance, serviceProvider, oa);
                }
                catch (ParameterResolveException)
                {
                    DrawUsage();
                    throw;
                }
            }
            var value  = InternalInvoke();

            // wait for task
            if (value != null)
            {
                value = serviceProvider.GetValueOrAwaitableResult(value, true);
            }
            if (session.Argv.IsAllUsed())
            {
                return value;
            }
            if (value != null)
            {
                if (((ITypeConfiguration) serviceProvider.GetRequiredService(
                        typeof(TypeConfiguration<>).FastMakeGenericType(value.GetType()))
                    ).IsDefinedCommand)
                {
                    session.Argv.Grouped();
                    var router = new CommandRouterBuilder(value)
                        .Build(serviceProvider.GetRequiredService<IServiceProvider>());
                    return router.Execute(serviceProvider);
                }
            }
            return session.UnknownArguments<object>();
        }

        public ICommandProperties Properties { get; }

        public OverrideArguments ResolveArguments(IServiceProvider serviceProvider, IArgumentList args)
        {
            var oa = new OverrideArguments();
            if (this.Parameters.Count > 0)
            {
                var avs = new ReadOnlyCollection<ArgumentValue>(
                    this.Parameters.Where(z => !z.IsAutoPadding)
                        .Select(z => new ArgumentValue(z)).ToArray()
                );
                var parser = serviceProvider.GetRequiredService<IArgumentParser>();
                parser.Parse(args, avs);
                foreach (var item in avs.Where(z => z.IsSetedValue()))
                {
                    item.Verify();
                    oa.AddArgument(item.Parameter.ParameterInfo.Name, item);
                }
            }
            return oa;
        }

        public abstract object Invoke(object instance, IServiceProvider serviceProvider, OverrideArguments args);
    }
}
