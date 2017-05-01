using System;

namespace Jasily.Frameworks.Cli.Attributes
{
    public abstract class BaseCommandAttribute : Attribute
    {
        /// <summary>
        /// name for command.
        /// </summary>
        public string[] Names { get; set; }
    }
}
