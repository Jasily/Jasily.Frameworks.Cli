using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal class PropertiesConfigurator : IPropertiesConfigurator
    {
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();
    }
}