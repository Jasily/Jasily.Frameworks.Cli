using System;
using System.Linq;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// the base command attribute.
    /// </summary>
    public abstract class BaseCommandAttribute : Attribute,
        IConfigureableAttribute<INameConfigurator>
    {
        /// <summary>
        /// name for command.
        /// </summary>
        public string[] Names { get; set; }

        /// <summary>
        /// should router ignore declaring name.
        /// </summary>
        public bool IgnoreDeclaringName { get; set; }

        public void Apply(INameConfigurator configurator)
        {
            foreach (var name in this.Names ?? Enumerable.Empty<string>())
            {
                configurator.AddName(name);
            }

            if (this.IgnoreDeclaringName)
            {
                configurator.IgnoreDeclaringName();
            }
        }
    }
}
