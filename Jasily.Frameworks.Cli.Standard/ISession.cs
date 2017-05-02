using System;
using System.Collections.Generic;

namespace Jasily.Frameworks.Cli
{
    /// <summary>
    /// cli session interface.
    /// </summary>
    public interface ISession : IDisposable
    {
        IReadOnlyList<string> OriginalArgv { get; }

        IArgumentList Argv { get; }

        void DrawUsage();

        void Termination();
    }
}
