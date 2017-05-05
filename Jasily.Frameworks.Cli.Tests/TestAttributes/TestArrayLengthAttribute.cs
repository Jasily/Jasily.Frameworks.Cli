using Jasily.Frameworks.Cli.Attributes.Parameters;
using Jasily.Frameworks.Cli.Tests.Olds;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests.TestAttributes
{
    [TestClass]
    public sealed class TestArrayLengthAttribute : BaseTest
    {
        public class TestClassParamsArray
        {
            public bool Func([ArrayLength(MinLength = 1)] params string[] names)
            {
                Assert.AreEqual(1, names.Length);
                Assert.AreEqual("1", names[0]);
                return true;
            }
        }

        [TestMethod]
        public void Test()
        {
            var e = this.Fire(new TestClassParamsArray(), out var _);
            Assert.AreEqual(true, e.Execute(new[] { nameof(TestClassParamsArray.Func), "1" }).Value);
            Assert.AreEqual(null, e.Execute(new[] { nameof(TestClassParamsArray.Func) }).Value);
        }
    }
}
