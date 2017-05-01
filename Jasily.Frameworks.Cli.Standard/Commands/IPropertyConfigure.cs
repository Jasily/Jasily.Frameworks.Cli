using System.Reflection;

namespace Jasily.Frameworks.Cli.Commands
{
    internal interface IPropertyConfigure
    {
        PropertyInfo Property { get; }

        bool HasCommandPropertyAttribute { get; }
    }
}
