using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli.Attributes
{
    public interface IConfigureableAttribute<in T>
    {
        /// <summary>
        /// apply to configuration.
        /// </summary>
        /// <param name="configuration"></param>
        void Apply(T configuration);
    }
}
