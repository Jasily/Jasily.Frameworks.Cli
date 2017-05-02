using Jasily.Frameworks.Cli.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    [TestClass]
    public sealed class TestInherit : BaseTest
    {
        public class Class1
        {
            public int Value() => 1;

            public void _() { }
        }

        public class Class2 : Class1
        {
        }

        public class Class3 : Class1
        {
            public new int Value() => 3;

            public void _() { }
        }

        [TestMethod]
        public void TestMethodNotInherit()
        {
            foreach (var item in this.Fire<Class1>())
            {
                Assert.AreEqual(1, item.Execute(new[] { nameof(Class1.Value) }));
            }

            foreach (var item in this.Fire<Class2>())
            {
                Assert.AreEqual(null, item.Execute(new[] { nameof(Class2.Value) }));
            }

            foreach (var item in this.Fire<Class3>())
            {
                Assert.AreEqual(3, item.Execute(new[] { nameof(Class3.Value) }));
            }
        }

        public class Class4
        {
            [CommandMethod]
            public int Value() => 1;

            [CommandMethod]
            public void _() { }
        }

        public class Class5 : Class4
        {
        }

        [TestMethod]
        public void TestMethodInheritOnlyContainsAttribute()
        {
            foreach (var item in this.Fire<Class4>())
            {
                Assert.AreEqual(1, item.Execute(new[] { nameof(Class4.Value) }));
            }

            foreach (var item in this.Fire<Class5>())
            {
                Assert.AreEqual(1, item.Execute(new[] { nameof(Class5.Value) }));
            }
        }
    }
}
