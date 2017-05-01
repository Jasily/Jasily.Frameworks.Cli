using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    [TestClass]
    public class TestCommandClass1 : BaseTest
    {
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
