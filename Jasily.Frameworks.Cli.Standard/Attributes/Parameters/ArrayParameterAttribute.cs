using System;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Attributes.Parameters
{
    /// <summary>
    /// this attribute only effect array typed parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ArrayParameterAttribute : Attribute,
        IConfigureableAttribute<IArrayParameterConfigurator>
    {
        /// <summary>
        /// valid value is (0,)
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// valid value is (0,)
        /// </summary>
        public int MaxLength { get; set; }

        /// <inheritdoc />
        public void Apply(IArrayParameterConfigurator configurator)
        {
            configurator.MaxLength = this.MaxLength;
            configurator.MinLength = this.MinLength;
        }
    }
}
