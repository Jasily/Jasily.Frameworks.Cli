using System;
using System.Collections.Generic;
using System.Text;
using Jasily.Frameworks.Cli.Configurations;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// declare the class contains command method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandClassAttribute : BaseCommandAttribute,
        IConfigureableAttribute<ICommandClassConfigurator>
    {
        /// <summary>
        /// Whether the command is cannot be the result.
        /// If true, the object will not return to outside, and display a usage.
        /// </summary>
        public bool IsNotResult { get; set; }

        public void Apply([NotNull] ICommandClassConfigurator configurator)
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));
            configurator.IsNotResult = this.IsNotResult;
        }
    }
}
