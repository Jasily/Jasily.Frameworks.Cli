using System.Reflection;

namespace Jasily.Frameworks.Cli.Configures
{
    internal interface IPropertyConfigure
    {
        PropertyInfo Property { get; }

        bool HasCommandPropertyAttribute { get; }
    }
}
