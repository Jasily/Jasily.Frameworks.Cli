using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jasily.Frameworks.Cli.Configures
{
    internal class NameConfiguration : INameConfiguration
    {
        private readonly List<string> _names = new List<string>();

        public NameConfiguration()
        {
            this.Names = new ReadOnlyCollection<string>(this._names);
        }

        public void AddName(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) return;
            this._names.Add(name.Trim().Replace(' ', '-'));
        }

        public IReadOnlyList<string> Names { get; }
    }
}
