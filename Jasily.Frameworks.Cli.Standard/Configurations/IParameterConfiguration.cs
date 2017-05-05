using Jasily.Frameworks.Cli.Converters;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal interface IParameterConfiguration : IParameterProperties
    {
        IValueConverter ValueConverter { get; }

        bool IsMatchName(string name);

        /// <summary>
        /// <exception cref="InvalidArgumentException"></exception>
        /// </summary>
        /// <param name="value"></param>
        void Check(object value);
    }
}