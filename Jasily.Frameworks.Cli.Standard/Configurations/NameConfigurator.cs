using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal class NameConfigurator : INameConfigurator
    {
        private readonly List<string> _names = new List<string>();

        public NameConfigurator()
        {
            this.Names = new ReadOnlyCollection<string>(this._names);
        }

        public void AddName(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) return;
            this._names.Add(name.Trim().Replace(' ', '-'));
        }

        public void IgnoreDeclaringName()
        {
            this.IsIgnoreDeclaringName = true;
        }

        public IReadOnlyList<string> Names { get; }

        public bool IsIgnoreDeclaringName { get; private set; }

        public IReadOnlyList<string> BuildName(string declaringName)
        {
            var names = new List<string>();
            if (!this.IsIgnoreDeclaringName || this._names.Count == 0)
            {
                names.Add(declaringName);
            }
            names.AddRange(this._names);
            return names.AsReadOnly();
        }
    }
}
