using Jasily.Frameworks.Cli.Commands;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal interface ICommandConfiguration : ICommandProperties
    {
        bool IsDefinedCommand { get; }
    }
}