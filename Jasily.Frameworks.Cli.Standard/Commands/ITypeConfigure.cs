using System;
using System.Reflection;

namespace Jasily.Frameworks.Cli.Commands
{
    internal interface ITypeConfigure
    {
        Type Type { get; }

        bool HasCommandClassAttribute { get; }

        ITypeConfigure GetInheritedTypeConfigure(Type declaringType);

        IPropertyConfigure GetConfigure(PropertyInfo property);
    }
}
