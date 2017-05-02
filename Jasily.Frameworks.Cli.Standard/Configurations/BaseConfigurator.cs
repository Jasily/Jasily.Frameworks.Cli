using System;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal class BaseConfigurator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        protected static string GetSafeName([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("value cannot be empty.");
            return  name.Trim().Replace(' ', '-');
        }
    }
}