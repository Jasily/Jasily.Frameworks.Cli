using System;
using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Configurations
{
    public interface INameConfigurator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="name"></param>
        void AddName(string name);

        /// <summary>
        /// ignore declaring C# name.
        /// </summary>
        void IgnoreDeclaringName();

        IReadOnlyList<string> Names { get; }
    }
}
