using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    /// <summary>
    /// base properties.
    /// </summary>
    public interface IBaseProperties
    {
        [NotNull]
        IReadOnlyList<string> Names { get; }

        [NotNull]
        string this[[NotNull] string propertyName] { get; }
    }
}