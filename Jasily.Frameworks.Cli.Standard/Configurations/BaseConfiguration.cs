using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Frameworks.Cli.Attributes;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal abstract class BaseConfiguration : IBaseProperties
    {
        private readonly List<string> _names = new List<string>();

        protected BaseConfiguration()
        {
            this.Names = this._names.AsReadOnly();
        }

        [NotNull]
        protected static T ConfigureConfigurator<T>([NotNull] Attribute[] attributes, [NotNull] T configurator)
        {
            attributes.OfType<IConfigureableAttribute<T>>().ForEach(z => z.Apply(configurator));
            return configurator;
        }

        protected virtual void Configure(Attribute[] attributes, string declaringName)
        {
            var nc = ConfigureConfigurator(attributes, new NameConfigurator());
            this._names.AddRange(nc.CreateNameList(declaringName));

            var pc = ConfigureConfigurator(attributes, new PropertiesConfigurator());
            this.Properties.AddRange(pc.Properties);
        }

        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public IReadOnlyList<string> Names { get; }

        public string this[[NotNull] string propertyName]
        {
            get
            {
                if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
                return this.Properties.TryGetValue(propertyName, out var value) ? value : string.Empty;
            }
        }
    }
}