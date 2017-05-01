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

        public ArgumentList([NotNull] StringComparer comparer)
        {
            this.stringComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
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

        public void Use(int count)
        {
            // I NEED TO impl some kind of `cli object 正义 add-property NAME VALUE` 变量在命令名之前。
            if (this.UsedArgvCount + count > this.Argv.Count) throw new InvalidOperationException();
            this.UsedArgvCount += count;
        }
    }
}
