using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Attributes.Properties
{
    public sealed class DescriptionAttribute : PropertyAttribute
    {
        public DescriptionAttribute(string value)
            : base((string) KnownPropertiesNames.Description, value)
        {
            
        }
    }
}