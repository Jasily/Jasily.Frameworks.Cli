using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Jasily.Frameworks.Cli.Attributes;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Core
{
    internal class ArgumentList : IArgumentList
    {
        private readonly List<ReadOnlyCollection<string>> _groups;

        public ArgumentList(SessionConfigurator configurator)
        {
            this.Argv = configurator.Argv.ToArray().AsReadOnly();
            this._groups = new List<ReadOnlyCollection<string>>();
            this.Groups = new ReadOnlyCollection<ReadOnlyCollection<string>>(this._groups);
        }

        public IReadOnlyList<string> Argv { get; }

        public int UsedArgvCount { get; private set; }

        public IReadOnlyList<IReadOnlyList<string>> Groups { get; }

        public void Use(int count)
        {
            if (this.UsedArgvCount + count > this.Argv.Count) throw new InvalidOperationException();
            this.UsedArgvCount += count;
        }

        public void Grouped()
        {
            var count = this._groups.Sum(z => z.Count);
            this._groups.Add(new ReadOnlyCollection<string>(this.Argv.Skip(count).Take(this.UsedArgvCount - count).ToArray()));
        }
    }
}
