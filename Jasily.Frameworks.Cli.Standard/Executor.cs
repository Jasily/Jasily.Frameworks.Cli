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
        private readonly Engine _engint;
        private readonly CommandRouter _routerRoot;

        internal Executor(Engine engint, CommandRouter router)
        {
            this._engint = engint;
            this._routerRoot = router;
        }

        public object Execute([NotNull, ItemNotNull] string[] argv)
        {
            if (argv == null) throw new ArgumentNullException(nameof(argv));
            if (argv.Any(z => z == null))
            {
                throw new ArgumentException($"Elements in <{nameof(argv)}> Cannot be Null.", nameof(argv));
            }
            if (this._engint == null || this._routerRoot == null)
            {
                throw new InvalidOperationException($"{nameof(Executor)} should Create by {nameof(Engine)}.");
            }

            using (var s = ServiceProviderServiceExtensions.CreateScope(this._engint.ServiceProvider))
            {
                var session = (Session)s.ServiceProvider.GetRequiredService<ISession>();
                session.OriginalArgv = new ReadOnlyCollection<string>(argv);
                var args = (ArgumentList)s.ServiceProvider.GetRequiredService<IArgumentList>();
                args.SetArgv(argv);
                session.Argv = args;
                try
                {
                    var value = this._routerRoot.Execute(s.ServiceProvider);
                    if (value != null)
                    {
                        var formater = ServiceProviderServiceExtensions.GetRequiredService<IValueFormater>(this._engint.ServiceProvider);
                        ServiceProviderServiceExtensions.GetRequiredService<IOutputer>(this._engint.ServiceProvider).WriteLine(OutputLevel.Normal, formater.Format(value));
                    }
                    return value;
                }
                catch (TerminationException)
                {
                    // ignore.
                }
                catch (CliException e)
                {
                    ServiceProviderServiceExtensions.GetRequiredService<IOutputer>(this._engint.ServiceProvider).WriteLine(OutputLevel.Error, e.Message);
                    session.DrawUsage();
                }
                catch (NotImplementedException e)
                {
                    ServiceProviderServiceExtensions.GetRequiredService<IOutputer>(this._engint.ServiceProvider).WriteLine(OutputLevel.Error, e.ToString());
                }
                catch (InvalidOperationException e)
                {
                    ServiceProviderServiceExtensions.GetRequiredService<IOutputer>(this._engint.ServiceProvider).WriteLine(OutputLevel.Error, e.Message);
                }
                catch (Exception e)
                {
                    ServiceProviderServiceExtensions.GetRequiredService<IOutputer>(this._engint.ServiceProvider).WriteLine(OutputLevel.Error, e.ToString());
                }
            }

            return null;
        }
    }
}