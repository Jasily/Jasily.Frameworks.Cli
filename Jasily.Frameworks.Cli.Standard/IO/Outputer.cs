using System;
using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.IO
{
    internal class Outputer : IOutputer
    {
        private readonly IEnumerable<IOutput> outputs;

        public Outputer(IEnumerable<IOutput> outputs)
        {
            this.outputs = outputs;
        }

        public void Write(OutputLevel level, string value)
        {
            foreach (var o in this.outputs)
            {
                o.Write(level, value);
            }
        }

        public void WriteLine(OutputLevel level, string value)
        {
            foreach (var o in this.outputs)
            {
                o.WriteLine(level, value);
            }
        }
    }
}
