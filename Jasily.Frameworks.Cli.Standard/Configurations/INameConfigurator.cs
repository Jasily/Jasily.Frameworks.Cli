using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    /// <summary>
    /// name configurator
    /// </summary>
    public interface INameConfigurator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="name"></param>
        void AddName([NotNull] string name);

        /// <summary>
        /// ignore declaring C# name.
        /// </summary>
        void IgnoreDeclaringName();

        IReadOnlyList<string> Names { get; }
    }
}
