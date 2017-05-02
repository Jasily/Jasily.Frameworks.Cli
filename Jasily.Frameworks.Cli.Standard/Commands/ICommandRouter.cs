using System.Collections.Generic;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Commands
{
    public interface ICommandRouter
    {
        IReadOnlyCollection<ICommandProperties> Commands { get; }
    }
}
