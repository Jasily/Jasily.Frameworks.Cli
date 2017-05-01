using System;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Attributes;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    public class BaseTest
    {
        protected IEnumerable<IEngine> Build<T>() where T : new ()
        {
            yield return new EngineBuilder()
                .AddInstance(new T())
                .InstallNetFramework()
                .InstallConsoleOutput()
                .Build();
            yield return new EngineBuilder()
                .AddType(typeof(T))
                .InstallNetFramework()
                .InstallConsoleOutput()
                .Build();
        }

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

        /// <summary>
        /// just <see cref="CommandClass"/> with <see cref="CommandClassAttribute"/> attribute
        /// </summary>
        [CommandClass]
        public class CommandClass2 : CommandClass
        {
            
        }

        public class AsyncCommandClass1
        {
            public async Task<CommandClass> GetCommandClass()
            {
                await Task.Run(() => { });
                return new CommandClass();
            }

            public async Task<CommandClass> GetNullCommandClass()
            {
                await Task.Run(() => { });
                return null;
            }

            public async Task<CommandClass2> GetCommandClass2()
            {
                await Task.Run(() => { });
                return new CommandClass2();
            }

            public async Task<CommandClass2> GetCommandClass2_Null()
            {
                await Task.Run(() => { });
                return null;
            }
        }
    }

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
            foreach (var item in this.Build<TestClassParamsArray>())
            {
                Assert.AreEqual(null, item.Execute(new string[] { nameof(TestClassParamsArray.Func), "1" }));
                Assert.AreEqual(null, item.Execute(new string[] { nameof(TestClassParamsArray.Func)}));
            }
        }
    }
}
