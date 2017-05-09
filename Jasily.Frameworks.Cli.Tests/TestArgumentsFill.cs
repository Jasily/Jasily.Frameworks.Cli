using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Tests.Olds;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    [TestClass]
    public class TestArgumentsFill : BaseTest
    {
        [CommandClass]
        public class TestClass1
        {
            public object Func0(string[] arg)
            {
                Assert.AreEqual(3, arg.Length);
                return null;
            }

            public TestClass1 Func1(string[] arg)
            {
                Assert.AreEqual(1, arg.Length);
                return this;
            }

            public int Func2(string[] arg)
            {
                Assert.AreEqual(1, arg.Length);
                return 5;
            }
        }

        [TestMethod]
        public void TestArgumentsStop()
        {
            var executor = this.Fire(new TestClass1(), out var _);

            Assert.AreEqual(null, executor.Execute(new[] { nameof(TestClass1.Func0), "true", nameof(TestClass1.Func2), "false" }).Value);
            Assert.AreEqual(5, executor.Execute(new[] { nameof(TestClass1.Func1), "true", "\\", nameof(TestClass1.Func2), "false" }).Value);
        }
    }
}