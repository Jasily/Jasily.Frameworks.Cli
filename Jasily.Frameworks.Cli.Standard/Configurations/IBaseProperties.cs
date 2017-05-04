using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    /// <summary>
    /// base properties.
    /// </summary>
    public interface IBaseProperties
    {
        /// <summary>
        /// All indexed names.
        /// </summary>
        [NotNull]
        IReadOnlyList<string> Names { get; }

        /// <summary>
        /// Get property or empty string.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        [NotNull]
        string this[[NotNull] string propertyName] { get; }
    }
}