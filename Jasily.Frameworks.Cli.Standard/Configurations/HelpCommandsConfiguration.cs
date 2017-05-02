using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal class HelpCommandsConfiguration
    {
        public HelpCommandsConfiguration([NotNull] StringComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            this.Commands = new HashSet<string>(comparer);
        }

        public HashSet<string> Commands { get; }
    }
}