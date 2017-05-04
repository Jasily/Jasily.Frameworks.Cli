using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    public static class PropertiesConfiguratorExtensions
    {
        public static void AddIfNotExists([NotNull] this IPropertiesConfigurator configurator, [NotNull] string name, [NotNull] string value)
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            configurator.Properties.GetValueOrAdd(name, value);
        }
    }
}