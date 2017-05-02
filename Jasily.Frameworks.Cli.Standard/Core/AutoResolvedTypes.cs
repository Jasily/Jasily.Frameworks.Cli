using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Core
{
    /// <summary>
    /// this should be singleton.
    /// </summary>
    internal class AutoResolvedTypes
    {
        [NotNull]
        private readonly HashSet<Type> _types;

        public AutoResolvedTypes()
        {
            this._types = new HashSet<Type>();
        }

        public AutoResolvedTypes(IEnumerable<Type> types)
        {
            this._types = new HashSet<Type>(types);
        }

        public void Add(Type type)
        {
            this._types.Add(type);
        }

        public bool Contains(Type type)
        {
            return this._types.Contains(type);
        }

        public AutoResolvedTypes Clone()
        {
            return new AutoResolvedTypes(this._types);
        }
    }
}