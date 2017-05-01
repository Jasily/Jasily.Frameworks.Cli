using System;

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
    }
}
