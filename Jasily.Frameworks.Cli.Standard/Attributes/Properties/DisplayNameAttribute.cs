using System.Collections.Generic;
using System.Text;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Attributes.Properties
{
    public sealed class DisplayNameAttribute : PropertyAttribute
    {
        public DisplayNameAttribute(string value)
            : base(KnownPropertiesNames.DisplayName, value)
        {
        }
    }
}
