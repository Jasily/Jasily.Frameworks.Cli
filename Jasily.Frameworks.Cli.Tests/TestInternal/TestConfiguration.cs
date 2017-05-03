using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Tests.Olds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests.TestInternal
{
    [TestClass]
    public class TestConfiguration
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

        public class Class004
        {
            [CommandMethod]
            public void Method1() {  }

            public void Method2() {  }
        }

        [TestMethod]
        public void TypeConfigurationLoaded()
        {
            new EngineBuilder().Build(out var provider);
            Assert.AreEqual(false, provider.GetRequiredService<TypeConfiguration<Class001>>().IsDefinedCommand);
            Assert.AreEqual(true, provider.GetRequiredService<TypeConfiguration<Class002>>().IsDefinedCommand);
            Assert.AreEqual(true, provider.GetRequiredService<TypeConfiguration<Class003>>().GetConfigure(
                typeof(Class003).GetProperty(nameof(Class003.Property1))).IsDefinedCommand);
            Assert.AreEqual(false, provider.GetRequiredService<TypeConfiguration<Class003>>().GetConfigure(
                typeof(Class003).GetProperty(nameof(Class003.Property2))).IsDefinedCommand);
            Assert.AreEqual(true, provider.GetRequiredService<TypeConfiguration<Class004>>().GetConfigure(
                typeof(Class004).GetMethod(nameof(Class004.Method1))).IsDefinedCommand);
            Assert.AreEqual(false, provider.GetRequiredService<TypeConfiguration<Class004>>().GetConfigure(
                typeof(Class004).GetMethod(nameof(Class004.Method2))).IsDefinedCommand);
        }
    }
}
