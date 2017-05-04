using System;
using Jasily.Frameworks.Cli.Configurations;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Attributes.Parameters
{
    /// <summary>
    /// this attribute only effect bool type parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class BoolParameterAttribute : Attribute,
        IConfigureableAttribute<INameConfigurator>,
        IConfigureableAttribute<IBooleanParameterConfigurator>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="value"></param>
        public BoolParameterAttribute(bool value) => this.Value = value;

        public BoolParameterAttribute(bool value, params string[] flags)
        {
            this.Value = value;
            this.Flags = flags;
        }

        public bool Value { get; }

        public string[] Flags { get; }

        void IConfigureableAttribute<IBooleanParameterConfigurator>.Apply([NotNull] IBooleanParameterConfigurator configurator)
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));
            if (this.Flags != null)
            {
                foreach (var keyword in this.Flags)
                {
                    if (this.Value) configurator.AddTrue(keyword);
                    else configurator.AddFalse(keyword);
                }
            }
        }

        public void Apply(INameConfigurator configurator)
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));
            if (this.Flags != null)
            {
                foreach (var keyword in this.Flags)
                {
                    configurator.AddName(keyword);
                }
            }
        }
    }
}
