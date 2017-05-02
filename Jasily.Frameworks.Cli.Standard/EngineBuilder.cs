using System;
using System.Linq;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Converters;
using Jasily.Frameworks.Cli.Core;
using Jasily.Frameworks.Cli.IO;
using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Commands;
using JetBrains.Annotations;
using System.Collections.Generic;
using Jasily.DependencyInjection.AwaiterAdapter;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli
{
    /// <summary>
    /// create a engine builder.
    /// </summary>
    public class EngineBuilder
    {
        private struct Element
        {
            private bool _isType;
            private Type _type;
            private object _instance;

            public static Element CreateTypeElement(Type type)
            {
                return new Element()
                {
                    _isType = true,
                    _type = type
                };
            }

            public static Element CreateInstanceElement(object instance)
            {
                return new Element()
                {
                    _isType = false,
                    _instance = instance
                };
            }

            public object GetValue(IServiceProvider provider)
            {
                return this._isType ? provider.GetRequiredService(this._type) : this._instance;
            }
        }

        private readonly List<Element> _types = new List<Element>();
        private readonly ServiceCollection _services = new ServiceCollection();
        private readonly AutoResolvedTypes _autoResolvedTypes = new AutoResolvedTypes();

        /// <summary>
        /// ctor.
        /// </summary>
        public EngineBuilder()
        {
            this._services.UseMethodInvoker(); // invoker
            this._services.UseAwaiterAdapter(); // wait for task

            // internal
            this._services.AddSingleton(typeof(TypeConfiguration<>));

            // base
            this._services.AddSingleton(StringComparer.OrdinalIgnoreCase);

            // mapper
            this._services.AddSingleton(typeof(ClassCommand<>));

            // core
            this.AddAutoResolvedSingleton<IEngine, Engine>();
            this.AddAutoResolvedScoped<ISession, Session>();
            this._services.AddScoped<IArgumentList, ArgumentList>();
            this._services.AddScoped(typeof(InstanceContainer<>));
            this.AddAutoResolvedTransient<IOutputer, Outputer>();

            // configureable
            this._services.AddTransient<IArgumentParser, ArgumentParser>();
            this._services.AddTransient<IUsageDrawer, UsageDrawer>();

            // converters
            this._services.InstallValueConverter();

            // formater
            this._services.AddTransient<IValueFormater, ObjectFormater>();
        }

        private void AddAutoResolvedSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            this._autoResolvedTypes.Add(typeof(TService));
            this._services.AddSingleton<TService, TImplementation>();
        }

        private void AddAutoResolvedScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            this._autoResolvedTypes.Add(typeof(TService));
            this._services.AddScoped<TService, TImplementation>();
        }

        private void AddAutoResolvedTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            this._autoResolvedTypes.Add(typeof(TService));
            this._services.AddTransient<TService, TImplementation>();
        }

        /// <summary>
        /// use customized <see cref="StringComparer"/>.
        /// </summary>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public EngineBuilder Use([NotNull] StringComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            this._services.AddSingleton(comparer);
            return this;
        }

        /// <summary>
        /// use customized <see cref="IArgumentParser"/>.
        /// lifetime of parser should be <see cref="ServiceLifetime.Transient"/>.
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public EngineBuilder Use([NotNull] IArgumentParser parser)
        {
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            this._services.AddSingleton(parser);
            return this;
        }

        /// <summary>
        /// lifetime of parser should be <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public EngineBuilder Use<T>([NotNull] Converters.IValueConverter<T> converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            this._services.AddSingleton(typeof(Converters.IValueConverter<T>), converter);
            return this;
        }

        public EngineBuilder Use([NotNull] IOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            this._services.AddSingleton(output);
            return this;
        }

        /// <summary>
        /// build engine.
        /// </summary>
        /// <returns></returns>
        public IEngine Build()
        {
            return this.Build(out var _);
        }

        /// <summary>
        /// use for unittest
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        internal IEngine Build(out IServiceProvider serviceProvider)
        {
            this._services.AddSingleton(this._autoResolvedTypes.Clone());
            var provider = this._services.BuildServiceProvider();            
            var engine = (Engine)provider.GetRequiredService<IEngine>();
            var builder = new CommandRouterBuilder(this._types.Select(z => z.GetValue(provider)).ToArray());
            serviceProvider = provider;
            return engine.Initialize(builder.Build(provider));
        }

        /// <summary>
        /// provide a static for easy to use the engine.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static object Fire([NotNull] object instance, [NotNull] string[] argv)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (argv == null) throw new ArgumentNullException(nameof(argv));

            return new EngineBuilder().Build().Fire(instance).Execute(argv);
        }
    }
}
