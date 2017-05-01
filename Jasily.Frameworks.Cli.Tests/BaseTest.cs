using System;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Attributes;
using System.Threading.Tasks;

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
}
