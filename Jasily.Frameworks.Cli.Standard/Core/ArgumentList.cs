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
        private readonly StringComparer stringComparer;
        private readonly List<ReadOnlyCollection<string>> groups;

        public ArgumentList([NotNull] StringComparer comparer)
        {
            this.stringComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            this.groups = new List<ReadOnlyCollection<string>>();
            this.Groups = new ReadOnlyCollection<ReadOnlyCollection<string>>(this.groups);
        }

        internal void SetArgv(string[] argv)
        {
            if (argv == null) throw new ArgumentNullException(nameof(argv));
            if (argv.Any(z => z == null)) throw new ArgumentException(nameof(argv));
            if (this.Argv != null) throw new InvalidOperationException();

            this.Argv = new ReadOnlyCollection<string>(argv.ToArray());
        }

        public IReadOnlyList<string> Argv { get; private set; }

        public int UsedArgvCount { get; private set; }

        public IReadOnlyList<IReadOnlyList<string>> Groups { get; }

        public void Use(int count)
        {
            if (this.UsedArgvCount + count > this.Argv.Count) throw new InvalidOperationException();
            this.UsedArgvCount += count;
        }

        public void Grouped()
        {
            var count = this.groups.Sum(z => z.Count);
            this.groups.Add(new ReadOnlyCollection<string>(this.Argv.Skip(count).Take(this.UsedArgvCount - count).ToArray()));
        }
    }
}
