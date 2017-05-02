using System;
using System.Linq;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Configures;

namespace Jasily.Frameworks.Cli.Attributes
{
    public abstract class BaseCommandAttribute : Attribute, IConfigureableAttribute<IApiCommandBuilder>,
        IConfigureableAttribute<INameConfiguration>
    {
        /// <summary>
        /// name for command.
        /// </summary>
        public string[] Names { get; set; }

        /// <summary>
        /// should router ignore declaring name.
        /// </summary>
        public bool IgnoreDeclaringName { get; set; }

        /// <summary>
        /// apply attribute to command.
        /// </summary>
        /// <param name="api"></param>
        public void Apply(IApiCommandBuilder api)
        {
            if (this.Names != null)
            {
                foreach (var name in this.Names)
                {
                    api.AddName(name);
                }
            }
        }

        public void Apply(INameConfiguration configuration)
        {
            foreach (var name in this.Names ?? Enumerable.Empty<string>())
            {
                configuration.AddName(name);
            }
        }
    }
}
