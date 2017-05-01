using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Core
{
    internal class Session : ISession
    {
        public IReadOnlyList<string> OriginalArgv { get; set; }

        public IArgumentList Argv { get; set; }

        public void Dispose()
        {
            
        }
    }
}
