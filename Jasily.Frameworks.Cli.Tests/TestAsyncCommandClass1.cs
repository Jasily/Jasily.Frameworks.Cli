using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    [TestClass]
    public class TestAsyncCommandClass1 : BaseTest
    {
        [TestMethod]
        public void Test()
        {
            foreach (var item in this.Build<AsyncCommandClass1>())
            {
                Assert.AreEqual(1, item.Execute(new string[] {
                    nameof(AsyncCommandClass1.GetCommandClass2),
                    nameof(CommandClass.Number),
                    "1" }));
                Assert.AreEqual(455, item.Execute(new string[] {
                    nameof(AsyncCommandClass1.GetCommandClass2),
                    nameof(CommandClass.Select),
                    "1", "2", "455" }));
            }
        }
    }
}
