using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Configurations
{
    /// <summary>
    /// Configurator for properties of command.
    /// </summary>
    public interface IPropertiesConfigurator
    {
        Dictionary<string, string> Properties { get; }
    }
}