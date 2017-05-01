using System;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Attributes;
using System.Threading.Tasks;
using System.Text;
using Jasily.Frameworks.Cli.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    public class BaseTest
    {
        private class TestOutput : IOutput
        {
            public StringBuilder Builder { get; } = new StringBuilder();

            public void Write(OutputLevel level, string value)
            {
                this.Builder.Append(value);
            }

            public void WriteLine(OutputLevel level, string value)
            {
                this.Builder.AppendLine(value);
            }
        }

        protected IEngine Build<T>(T instance, out StringBuilder outputStringBuilder)
        {
            var builder = new EngineBuilder();
            var output = new TestOutput();
            builder.Use(output);
            outputStringBuilder = output.Builder;
            return new EngineBuilder()
                .AddInstance(instance)
                .InstallNetFramework()
                .InstallConsoleOutput()
                .Build();
        }

        protected IEnumerable<IEngine> Build<T>() where T : new ()
        {
            yield return new EngineBuilder()
                .AddInstance(new T())
                .InstallNetFramework()
                .InstallConsoleOutput()
                .Build();
            yield return new EngineBuilder()
                .AddType(typeof(T))
                .InstallNetFramework()
                .InstallConsoleOutput()
                .Build();
        }
    }
}
