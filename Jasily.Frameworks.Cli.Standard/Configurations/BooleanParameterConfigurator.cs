using System;
using System.Collections.Generic;
using Jasily.Extensions.System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal class BooleanParameterConfigurator : BaseConfigurator, IBooleanParameterConfigurator
    {
        private readonly Dictionary<string, string> _flags;

        internal BooleanParameterConfigurator(StringComparer comparer)
        {
            this._flags = new Dictionary<string, string>(comparer);
        }

        public void AddTrue([NotNull] string flag)
        {
            flag = GetSafeName(flag);
            if (this._flags.ContainsKey(flag)) throw new InvalidOperationException("flag is already configurated.");
            this._flags.Add(flag, "true");
        }

        public void AddFalse([NotNull] string flag)
        {
            if (flag == null) throw new ArgumentNullException(nameof(flag));
            if (this._flags.ContainsKey(flag)) throw new InvalidOperationException("flag is already configurated.");
            this._flags.Add(flag, "false");
        }

        internal IReadOnlyDictionary<string, string> CreateMap()
        {
            return new Dictionary<string, string>(this._flags, this._flags.Comparer).AsReadOnly();
        }
    }
}