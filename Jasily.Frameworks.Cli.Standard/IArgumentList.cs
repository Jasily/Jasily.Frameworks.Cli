using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli
{
    public interface IArgumentList
    {
        IReadOnlyList<string> Argv { get; }

        int UsedArgvCount { get; }

        void Use(int count);

        void Grouped();

        IReadOnlyList<IReadOnlyList<string>> Groups { get; }
    }
}
