using System;
using System.Collections.Generic;
using System.Text;
using Jasily.Frameworks.Cli.IO;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Tests.Olds
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

        protected Executor Fire<T>([NotNull] T instance, out StringBuilder outputStringBuilder)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            var builder = new EngineBuilder();
            var output = new TestOutput();
            builder.Use(output);
            outputStringBuilder = output.Builder;
            return builder
                .InstallNetFramework()
                .InstallConsoleOutput()
                .Build()
                .Fire(instance);
        }

        protected IEnumerable<Executor> Fire<T>() where T : new ()
        {
            yield return new EngineBuilder()
                .InstallNetFramework()
                .InstallConsoleOutput()
                .Build()
                .Fire(new T());
        }
    }
}
