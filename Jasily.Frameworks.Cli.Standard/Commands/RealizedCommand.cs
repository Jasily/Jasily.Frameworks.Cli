using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Core;
using Jasily.Frameworks.Cli.Exceptions;
using Jasily.Frameworks.Cli.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Commands
{
    /// <summary>
    /// provide api when command build
    /// </summary>
    public interface IApiCommandBuilder
    {
        /// <summary>
        /// add name
        /// </summary>
        /// <param name="name"></param>
        void AddName(string name);
    }

    internal abstract class RealizedCommand : ICommandProperties, IApiCommandBuilder
    {
        private readonly bool ignoreDeclaringName;
        private readonly HashSet<string> namesSet = new HashSet<string>();
        private readonly List<string> names = new List<string>();
        private bool isFreeze;

        public RealizedCommand(BaseCommandAttribute attribute)
        {
            this.Names = new ReadOnlyCollection<string>(this.names);
            this.ignoreDeclaringName = attribute?.IgnoreDeclaringName ?? false;
            attribute?.Apply(this);
        }

        void IApiCommandBuilder.AddName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name cannot be empty.", nameof(name));
            if (this.isFreeze) throw new InvalidOperationException();
            this.names.Add(name.Trim());
        }

        public abstract IReadOnlyList<ParameterInfoDescriptor> Parameters { get; }

        public IReadOnlyList<string> Names { get; }

        public abstract string DeclaringName { get; }

        public bool IgnoreDeclaringName => this.ignoreDeclaringName && this.names.Count > 0;

        public virtual object Invoke(IServiceProvider serviceProvider, [NotNull] object instance)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            var session = serviceProvider.GetRequiredService<ISession>();

            void DrawUsage()
            {
                serviceProvider.GetRequiredService<IUsageDrawer>()
                    .DrawParameter(this, this.Parameters);
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
            else if (value != null && value.GetType().GetTypeInfo().GetCustomAttribute<CommandClassAttribute>() != null)
            {
                session.Argv.Grouped();
                var router = new CommandRouterBuilder(value)
                    .Build(serviceProvider.GetRequiredService<IServiceProvider>());
                return router.Execute(serviceProvider);
            }
            else
            {
                throw UnknownArgumentsException.Build(session);
            }
        }

        public abstract OverrideArguments ResolveArguments(IServiceProvider serviceProvider, IArgumentList args);

        public abstract object Invoke(object instance, IServiceProvider serviceProvider, OverrideArguments args);
    }

    internal abstract class RealizedCommand<TClass> : RealizedCommand
    {
        protected RealizedCommand(IServiceProvider serviceProvider, MethodBase method, BaseCommandAttribute attr)
            : base(attr)
        {
            this.Method = method;

            var comaprer = serviceProvider.GetRequiredService<StringComparer>();
            this.Parameters = method
                .GetParameters()
                .Select(z => new ParameterInfoDescriptor(z, comaprer))
                .ToArray();
        }

        public MethodBase Method { get; }

        public override IReadOnlyList<ParameterInfoDescriptor> Parameters { get; }

        public override OverrideArguments ResolveArguments(IServiceProvider serviceProvider, IArgumentList args)
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

        public override object Invoke(object instance, IServiceProvider serviceProvider, OverrideArguments args)
        {
            return this.Invoke((TClass)instance, serviceProvider, args);
        }

        public abstract object Invoke(TClass instance, IServiceProvider serviceProvider, OverrideArguments args);
    }
}
