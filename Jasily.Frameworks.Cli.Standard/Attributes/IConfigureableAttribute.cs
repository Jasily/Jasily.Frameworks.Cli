using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli.Attributes
{
    public interface IConfigureableAttribute<in T>
    {
        /// <summary>
        /// apply to configurator.
        /// </summary>
        /// <param name="configurator"></param>
        void Apply(T configurator);
    }
}
