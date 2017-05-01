using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Exceptions;
using Jasily.Frameworks.Cli.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Core
{
    internal class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;
        private CommandRouter routerRoot;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Engine Initialize(CommandRouter routerRoot)
        {
            this.routerRoot = routerRoot;
            return this;
        }

        public object Execute([NotNull][ItemNotNull] string[] argv)
        {
            if (argv == null) throw new ArgumentNullException(nameof(argv));
            if (argv.Any(z => z == null)) throw new ArgumentException("element in argv cannot be null.", nameof(argv));

            using (var s = this.serviceProvider.CreateScope())
            {
                var session = (Session) s.ServiceProvider.GetRequiredService<ISession>();
                session.OriginalArgv = new ReadOnlyCollection<string>(argv);                
                var args = (ArgumentList) s.ServiceProvider.GetRequiredService<IArgumentList>();
                args.SetArgv(argv);
                session.Argv = args;
                try
                {
                    var value = this.routerRoot.Execute(s.ServiceProvider);
                    if (value != null)
                    {
                        var formater = this.serviceProvider.GetRequiredService<IValueFormater>();
                        this.serviceProvider.GetRequiredService<IOutputer>().WriteLine(OutputLevel.Normal, formater.Format(value));
                    }
                    return value;
                }
                catch (ConvertException e)
                {
                    throw;
                }
                catch (CliException e)
                {
                    this.serviceProvider.GetRequiredService<IOutputer>().WriteLine(OutputLevel.Error, e.Message);
                }
            }

            return null;
        }

        private class E : Exception
        {

        }
    }
}
