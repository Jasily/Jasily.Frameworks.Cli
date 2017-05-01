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

namespace Jasily.Frameworks.Cli
{
    /// <summary>
    /// create a engine builder.
    /// </summary>
    public class EngineBuilder
    {
        private struct Element
        {
            public bool IsType;
            public Type Type;
            public object Instance;

            public static Element CreateTypeElement(Type type)
            {
                return new Element()
                {
                    IsType = true,
                    Type = type
                };
            }

            public static Element CreateInstanceElement(object instance)
            {
                return new Element()
                {
                    IsType = false,
                    Instance = instance
                };
            }

            public object GetValue(IServiceProvider provider)
            {
                return this.IsType ? provider.GetRequiredService(this.Type) : this.Instance;
            }
        }

        private readonly List<Element> types = new List<Element>();

        /// <summary>
        /// ctor.
        /// </summary>
        public EngineBuilder()
        {
            var sc = this.Services;
            sc.UseMethodInvoker();

            // base
            sc.AddSingleton(StringComparer.OrdinalIgnoreCase);

            // mapper
            sc.AddSingleton(typeof(ClassCommand<>));

            // core
            sc.AddSingleton<IEngine, Engine>();
            sc.AddScoped<ISession, Session>();
            sc.AddScoped<IArgumentList, ArgumentList>();
            sc.AddScoped(typeof(InstanceContainer<>));
            sc.AddTransient<IOutputer, Outputer>();

            // configureable
            sc.AddTransient<IArgumentParser, ArgumentParser>();
            sc.AddTransient<IUsageDrawer, UsageDrawer>();

            // converters
            sc.AddSingleton<IValueConverter<bool>, BooleanConverter>()
                .AddSingleton<IValueConverter<int>, Int32Converter>()
                .AddSingleton<IValueConverter<long>, Int64Converter>()
                .AddSingleton<IValueConverter<float>, FloatConverter>()
                .AddSingleton<IValueConverter<double>, DoubleConverter>()
                .AddSingleton<IValueConverter<string>, StringConverter>();
            
            // array converters
            sc.AddSingleton(typeof(ArrayConverter<>));

            sc.AddSingleton<IValueConverterFactory, ValueConverterFactory>();

            // formater
            sc.AddTransient<IValueFormater, ObjectFormater>();
        }

        private ServiceCollection Services { get; } = new ServiceCollection();

        /// <summary>
        /// add assembly to engine root.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public EngineBuilder AddAssembly([NotNull] Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            foreach (var type in assembly.DefinedTypes)
            {
                if (type.GetCustomAttribute<CommandClassAttribute>() != null) this.AddType(type.AsType());
            } 
            return this;
        }

        /// <summary>
        /// add type to engine root.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public EngineBuilder AddType(Type type)
        {
            this.Services.AddScoped(type);
            this.types.Add(Element.CreateTypeElement(type));
            return this;
        }

        public EngineBuilder AddInstance(object instance)
        {
            this.types.Add(Element.CreateInstanceElement(instance));
            return this;
        }

        /// <summary>
        /// use customized <see cref="StringComparer"/>.
        /// </summary>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public EngineBuilder Use([NotNull] StringComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            this.Services.AddSingleton(comparer);
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
            this.Services.AddSingleton(parser);
            return this;
        }

        /// <summary>
        /// lifetime of parser should be <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public EngineBuilder Use<T>([NotNull] IValueConverter<T> converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            this.Services.AddSingleton(typeof(IValueConverter<T>), converter);
            return this;
        }

        public EngineBuilder Use([NotNull] IOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            this.Services.AddSingleton(output);
            return this;
        }

        /// <summary>
        /// build engine.
        /// </summary>
        /// <returns></returns>
        public IEngine Build()
        {
            var sc = this.Services;
            var comparer = (StringComparer) sc.Last(z => z.ImplementationInstance is StringComparer).ImplementationInstance;
            var provider = sc.BuildServiceProvider();
            var engine = (Engine)provider.GetRequiredService<IEngine>();
            var builder = new CommandRouterBuilder(this.types.Select(z => z.GetValue(provider)).ToArray());
            return engine.Initialize(builder.Build(provider));
        }

        /// <summary>
        /// provide a static for easy to use the engine.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="argv"></param>
        public static object FireAssembly([NotNull] Assembly assembly, [NotNull] string[] argv)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (argv == null) throw new ArgumentNullException(nameof(argv));

            return new EngineBuilder().AddAssembly(assembly).Build().Execute(argv);
        }

        /// <summary>
        /// provide a static for easy to use the engine.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="argv"></param>
        public static object FireType([NotNull] Type type, [NotNull] string[] argv)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (argv == null) throw new ArgumentNullException(nameof(argv));

            return new EngineBuilder().AddType(type).Build().Execute(argv);
        }

        /// <summary>
        /// provide a static for easy to use the engine.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static object FireInstance([NotNull] object instance, [NotNull] string[] argv)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (argv == null) throw new ArgumentNullException(nameof(argv));

            return new EngineBuilder().AddInstance(instance).Build().Execute(argv);
        }
    }
}
