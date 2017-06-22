using System.Collections.Generic;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Core
{
    public interface IArgumentValue
    {
        string ParameterName { get; }
        IParameterProperties ParameterProperties { get; }
        IReadOnlyList<string> Values { get; }

        void AddValue(string value);

        bool IsAcceptValue();

        bool IsMatch(string name);

        bool IsSetedValue();

        /// <summary>
        /// get resolved value.
        /// </summary>
        /// <returns></returns>
        object GetValue();
    }
}