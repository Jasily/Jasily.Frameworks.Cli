using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Commands
{
    public interface ICommandRouter
    {
        IReadOnlyCollection<ICommandProperties> Commands { get; }
    }
}
