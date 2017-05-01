using System;

namespace Jasily.Frameworks.Cli.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class CommandParameterAttribute : Attribute
    {
        /// <summary>
        /// name for command.
        /// </summary>
        public string[] Names { get; set; }

        /// <summary>
        /// declare the parameter should not by user input.
        /// </summary>
        public bool IsAutoPadding { get; set; }
    }
}
