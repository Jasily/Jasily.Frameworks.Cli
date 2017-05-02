using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jasily.Frameworks.Cli.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Jasily.Frameworks.Cli.Commands;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Tests.TestInternal
{
    [TestClass]
    public class TestConfigure : BaseTest
    {
        public class Class001 { }

        [CommandClass]
        public class Class002 { }

        public class Class003
        {
            [CommandProperty]
            public string Property1 { get; }

            public string Property2 { get; }
        }

        [TestMethod]
        public void TypeConfigureLoaded()
        {
            var builder = new EngineBuilder();
            builder.Build(out var provider);
            Assert.AreEqual(false, provider.GetRequiredService<TypeConfiguration<Class001>>().IsDefinedCommand);
            Assert.AreEqual(true, provider.GetRequiredService<TypeConfiguration<Class002>>().IsDefinedCommand);
            Assert.AreEqual(true, provider.GetRequiredService<TypeConfiguration<Class003>>().GetConfigure(
                typeof(Class003).GetProperty("Property1")).IsDefinedCommand);
            Assert.AreEqual(false, provider.GetRequiredService<TypeConfiguration<Class003>>().GetConfigure(
                typeof(Class003).GetProperty("Property2")).IsDefinedCommand);
        }
    }
}
