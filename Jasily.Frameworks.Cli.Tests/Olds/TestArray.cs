using Jasily.Frameworks.Cli.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests.Olds
{
    [TestClass]
    public sealed class TestArray : BaseTest
    {
        public class TestClassParamsArray
        {
            public void Func([ArrayParameter(MinLength = 1)] params string[] names)
            {
                Assert.AreEqual(1, names.Length);
                Assert.AreEqual("1", names[0]);
            }

            public void Func2() { }
        }

        [TestMethod]
        public void Test()
        {
            foreach (var item in this.Fire<TestClassParamsArray>())
            {
                Assert.AreEqual(null, item.Execute(new[] { nameof(TestClassParamsArray.Func), "1" }));
                Assert.AreEqual(null, item.Execute(new[] { nameof(TestClassParamsArray.Func)}));
            }
        }
    }
}
