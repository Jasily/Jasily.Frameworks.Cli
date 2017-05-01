using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Commands
{
    public interface ICommandProperties
    {
        IEnumerable<string> EnumerateNames();
    }
}
