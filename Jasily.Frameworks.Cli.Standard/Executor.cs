using System;
using System.Collections.ObjectModel;
using System.Linq;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Core;
using Jasily.Frameworks.Cli.Exceptions;
using Jasily.Frameworks.Cli.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli
{
    public struct Executor
    {
        private readonly Engine _engine;

        internal Executor(Engine engine, object instance)
        {
            this._engine = engine;
            this.Value = instance;
        }

        public object Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argv"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public Executor Execute([NotNull, ItemNotNull] string[] argv)
        {
            if (argv == null) throw new ArgumentNullException(nameof(argv));
            if (argv.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentException($"Elements in <{nameof(argv)}> Cannot be Null Or Empty.", nameof(argv));
            }
            if (this._engine == null || this.Value == null)
            {
                throw new InvalidOperationException($"{nameof(Executor)} should Create by {nameof(Engine)}.");
            }
            if (this.Value == null)
            {
                throw new InvalidOperationException($"");
            }

            var router = CommandRouter.Build(this._engine.ServiceProvider, this.Value);

            using (var s = this._engine.ServiceProvider.CreateScope())
            {
                var session = (Session)s.ServiceProvider.GetRequiredService<ISession>();
                session.OriginalArgv = new ReadOnlyCollection<string>(argv);
                var args = (ArgumentList)s.ServiceProvider.GetRequiredService<IArgumentList>();
                args.SetArgv(argv);
                session.Argv = args;
                try
                {
                    var value = router.Execute(s.ServiceProvider);
                    if (value != null)
                    {
                        var formater = this._engine.ServiceProvider.GetRequiredService<IValueFormater>();
                        this._engine.ServiceProvider.GetRequiredService<IOutputer>()
                            .WriteLine(OutputLevel.Normal, formater.Format(value));
                    }
                    return new Executor(this._engine, value);
                }
                catch (TerminationException)
                {
                    // ignore.
                }
                catch (CliException e)
                {
                    if (e.Message.Length > 0)
                    {
                        this._engine.ServiceProvider.GetRequiredService<IOutputer>()
                            .WriteLine(OutputLevel.Error, e.Message);
                    }
                    
                    session.DrawUsage();
                }
                catch (NotImplementedException e)
                {
                    this._engine.ServiceProvider.GetRequiredService<IOutputer>()
                        .WriteLine(OutputLevel.Error, e.ToString());
                }
                catch (InvalidOperationException e)
                {
                    this._engine.ServiceProvider.GetRequiredService<IOutputer>()
                        .WriteLine(OutputLevel.Error, e.Message);
                }
                catch (Exception e)
                {
                    this._engine.ServiceProvider.GetRequiredService<IOutputer>()
                        .WriteLine(OutputLevel.Error, e.ToString());
                }
            }

            return new Executor(this._engine, null);
        }

        public override string ToString() => this.Value?.ToString() ?? string.Empty;
    }
}