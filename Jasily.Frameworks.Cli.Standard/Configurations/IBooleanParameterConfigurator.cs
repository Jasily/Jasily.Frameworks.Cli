using System;

namespace Jasily.Frameworks.Cli.Configurations
{
    /// <summary>
    /// bool typed parameter configurator
    /// </summary>
    public interface IBooleanParameterConfigurator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        void AddTrue(string flag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        void AddFalse(string flag);
    }
}