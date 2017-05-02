using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConfigureableAttribute<in T>
    {
        /// <summary>
        /// apply to configurator.
        /// </summary>
        /// <param name="configurator"></param>
        /// <exception cref="ArgumentNullException"></exception>
        void Apply([NotNull] T configurator);
    }
}
