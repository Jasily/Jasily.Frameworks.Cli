using Jasily.Frameworks.Cli.Converters;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal interface IParameterConfiguration : IParameterProperties
    {
        IValueConverter ValueConverter { get; }

        bool IsMatchName(string name);
    }
}