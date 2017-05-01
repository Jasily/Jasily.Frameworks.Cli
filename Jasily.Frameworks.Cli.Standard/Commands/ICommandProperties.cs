using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Commands
{
    /// <summary>
    /// properties of command for display usage.
    /// </summary>
    public interface ICommandProperties
    {
        /// <summary>
        /// declaring name for command (in c# code).
        /// </summary>
        string DeclaringName { get; }

        /// <summary>
        /// should router ignore declaring name.
        /// </summary>
        bool IgnoreDeclaringName { get; }

        /// <summary>
        /// all name which should map to router.
        /// </summary>
        IReadOnlyList<string> Names { get; }
    }
}
