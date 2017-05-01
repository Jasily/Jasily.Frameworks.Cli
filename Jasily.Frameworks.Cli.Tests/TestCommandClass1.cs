using System;
using Jasily.Frameworks.Cli.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    [TestClass]
    public class TestCommandClass1 : BaseTest
    {
        public class CommandClass
        {
            public int Number([CommandParameter(IsAutoPadding = true)] IServiceProvider provider, int value)
            {
                if (provider == null) throw new ArgumentNullException();
                return value;
            }

            public int Select(int val1, int val2, int val3)
            {
                return val3;
            }

            public string Usage(
                int val1,
                [CommandParameter(IsAutoPadding = true)] IServiceProvider provider,
                string val2,
                [CommandParameter(Names = new [] { "valx" })]
                double val3 = 15,
                params string[] val4)
            {
                throw new InvalidOperationException();
            }
        }

        [TestMethod]
        public void Test()
        {
            foreach (var item in this.Build<CommandClass>())
            {
                Assert.AreEqual(1, item.Execute(new string[] {
                    nameof(CommandClass.Number),
                    "1" }));
                Assert.AreEqual(455, item.Execute(new string[] {
                    nameof(CommandClass.Select),
                    "1", "2", "455" }));
            }
        }
    }
}
