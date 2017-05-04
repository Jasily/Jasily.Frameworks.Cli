using System;
using Jasily.Frameworks.Cli.Configurations;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Attributes.Properties
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PropertyAttribute : Attribute, IConfigureableAttribute<IPropertiesConfigurator>
    {
        public PropertyAttribute(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; }

        public string Value { get; }

        public void Apply([NotNull] IPropertiesConfigurator configurator)
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));

            configurator.Properties.Add(this.Name, this.Value);
        }
    }
}