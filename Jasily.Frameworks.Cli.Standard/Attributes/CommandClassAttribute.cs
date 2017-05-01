using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli.Attributes
{

    /// <summary>
    /// declare the class contains command method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandClassAttribute : Attribute
    {
        /// <summary>
        /// alias for command.
        /// </summary>
        public string[] Names { get; set; }
    }
}
