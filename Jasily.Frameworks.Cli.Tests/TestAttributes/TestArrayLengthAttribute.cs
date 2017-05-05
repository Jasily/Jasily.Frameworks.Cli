using Jasily.Frameworks.Cli.Attributes.Parameters;
using Jasily.Frameworks.Cli.Tests.Olds;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests.TestAttributes
{
    [TestClass]
    public sealed class TestArrayLengthAttribute : BaseTest
    {
        public class TestClass1
        {
            public bool Func1([ArrayLength(MinLength = 1)] params string[] names)
            {
                Assert.AreEqual(1, names.Length);
                Assert.AreEqual("1", names[0]);
                return true;
            }

            public bool Func2([ArrayLength(MinLength = 1)] string[] names)
            {
                Assert.AreEqual(1, names.Length);
                Assert.AreEqual("1", names[0]);
                return true;
            }
        }

        [TestMethod]
        public void TestArrayLengthAttributeMinLength()
        {
            var e = this.Fire(new TestClass1(), out var _);
            Assert.AreEqual(true, e.Execute(new[] { nameof(TestClass1.Func1), "1" }).Value);
            Assert.AreEqual(null, e.Execute(new[] { nameof(TestClass1.Func1) }).Value);
            Assert.AreEqual(true, e.Execute(new[] { nameof(TestClass1.Func2), "1" }).Value);
            Assert.AreEqual(null, e.Execute(new[] { nameof(TestClass1.Func2) }).Value);
        }

        public class TestClass2
        {
            public bool Func1([ArrayLength(MaxLength = 1)] params string[] names)
            {
                Assert.AreEqual(1, names.Length);
                Assert.AreEqual("1", names[0]);
                return true;
            }

            public bool Func2([ArrayLength(MaxLength = 1)] string[] names)
            {
                Assert.AreEqual(1, names.Length);
                Assert.AreEqual("1", names[0]);
                return true;
            }
        }

        [TestMethod]
        public void TestArrayLengthAttributeMaxLength()
        {
            var e = this.Fire(new TestClass2(), out var _);
            Assert.AreEqual(true, e.Execute(new[] { nameof(TestClass2.Func1), "1" }).Value);
            Assert.AreEqual(null, e.Execute(new[] { nameof(TestClass2.Func1), "1", "2" }).Value);
            Assert.AreEqual(true, e.Execute(new[] { nameof(TestClass2.Func2), "1" }).Value);
            Assert.AreEqual(null, e.Execute(new[] { nameof(TestClass2.Func2), "1", "2" }).Value);
        }
    }
}
