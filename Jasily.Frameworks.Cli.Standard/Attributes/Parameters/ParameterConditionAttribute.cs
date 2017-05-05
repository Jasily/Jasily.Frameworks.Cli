using System;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Exceptions;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Attributes.Parameters
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class ParameterConditionAttribute : Attribute, ICondition,
        IConfigureableAttribute<IParameterConfigurator>
    {
        public abstract void Check(object value);

        public void Apply([NotNull] IParameterConfigurator configurator)
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));
            configurator.AddCondition(this);
        }

        protected void InvalidArgument(string message)
        {
            throw new InvalidArgumentException(message);
        }

        public override string ToString() => string.Empty;
    }
}