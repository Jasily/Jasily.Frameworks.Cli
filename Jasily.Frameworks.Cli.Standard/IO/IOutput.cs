using System;
using System.Text;

namespace Jasily.Frameworks.Cli.IO
{
    public interface IOutput
    {
        void Write(OutputLevel level, string value);

        void WriteLine(OutputLevel level, string value);
    }
}
