using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Converters;
using Jasily.Frameworks.Cli.IO;

namespace Jasily.Frameworks.Cli
{
    public static class NetFrameworkExtensions
    {
        public static EngineBuilder InstallNetFramework(this EngineBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Use(new FileInfoConverter());
            builder.Use(new DirectoryInfoConverter());
            return builder;
        }

        public static EngineBuilder InstallConsoleOutput(this EngineBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            builder.Use(new ConsoleOutput());
            return builder;
        }
    }
}
