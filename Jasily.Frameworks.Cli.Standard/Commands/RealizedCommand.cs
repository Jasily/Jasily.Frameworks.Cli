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
    internal abstract class RealizedCommand : ICommandProperties
    {
        private readonly HashSet<string> namesSet = new HashSet<string>();
        private readonly List<string> names = new List<string>();

        public RealizedCommand(BaseCommandAttribute attr)
        {
            this.Names = new ReadOnlyCollection<string>(this.names);
            if (attr != null)
            {
                this.IgnoreDeclaringName = attr.IgnoreDeclaringName;
                this.AddName(attr.Names);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException">throw if any item of names is null.</exception>
        /// <param name="names"></param>
        protected void AddName(params string[] names)
        {
            if (names == null) return;
            foreach (var item in names)
            {
                if (this.namesSet.Add(item ?? throw new ArgumentException()))
                {

                }
            }
        }

        public IReadOnlyList<string> Names { get; }

        public abstract string DeclaringName { get; }

        public bool IgnoreDeclaringName { get; }

        public virtual object Invoke(IServiceProvider serviceProvider, [NotNull] object instance)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var session = serviceProvider.GetRequiredService<ISession>();
            var oa = this.ResolveArguments(serviceProvider, session.Argv);
            object value;
            try
            {
                value = this.Invoke(instance, serviceProvider, oa);
            }
            catch (ParameterResolveException)
            {
                throw new NotImplementedException();
            }
            // wait for task
            value = serviceProvider.GetValueOrAwaitableResult(value, true);
            if (session.Argv.IsAllUsed())
            {
                return value;
            }
            else if (value != null && value.GetType().GetTypeInfo().GetCustomAttribute<CommandClassAttribute>() != null)
            {
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

        public IReadOnlyList<ParameterInfoDescriptor> Parameters { get; }

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
