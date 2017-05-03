using System;
using System.Collections.Generic;

namespace Jasily.Frameworks.Cli
{
    /// <summary>
    /// cli session interface.
    /// </summary>
    public interface ISession : IDisposable
    {
        ExecuteMode ExecuteMode { get; }

        IArgumentList Argv { get; }

        void DrawUsage();

        void Termination();
    }
}
