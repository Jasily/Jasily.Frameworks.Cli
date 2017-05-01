using System;
using Jasily.Frameworks.Cli.Commands;

namespace Jasily.Frameworks.Cli.Attributes
{
    public abstract class BaseCommandAttribute : Attribute
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
        public virtual void Apply(IApiCommandBuilder api)
        {
            if (this.Names != null)
            {
                foreach (var name in this.Names)
                {
                    api.AddName(name);
                }
            }
        }
    }
}
