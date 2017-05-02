using System;
using System.Linq;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class CommandParameterAttribute : Attribute,
        IConfigureableAttribute<INameConfigurator>
    {
        /// <summary>
        /// name for command parameter.
        /// </summary>
        public string[] Names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurator"></param>
        public void Apply(INameConfigurator configurator)
        {
            foreach (var name in this.Names ?? Enumerable.Empty<string>())
            {
                configurator.AddName(name);
            }
        }
    }
}
