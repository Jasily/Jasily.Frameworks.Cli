using System;
using Jasily.Frameworks.Cli.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests.Olds
{
    [TestClass]
    public class TestCommandClass1 : BaseTest
    {
        public class CommandClass
        {
            public int Number(int value)
            {
                return value;
            }

            public int Select(int val1, int val2, int val3)
            {
                return val3;
            }

            public string Usage(
                int val1,
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
            foreach (var item in this.Fire<CommandClass>())
            {
                Assert.AreEqual(1, item.Execute(new[] {
                    nameof(CommandClass.Number),
                    "1" }).Value);
                Assert.AreEqual(455, item.Execute(new[] {
                    nameof(CommandClass.Select),
                    "1", "2", "455" }).Value);
            }
        }
    }
}
