using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Configurations
{
    public interface ICommandProperties
    {
        IReadOnlyList<string> Names { get; }
    }
}