using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Frameworks.Cli.Attributes;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal abstract class BaseConfiguration
    {
        protected static IReadOnlyList<string> CreateNameList(IEnumerable<Attribute> attributes, string declaringName)
        {
            var n = new NameConfigurator();
            foreach (var attrubute in attributes.OfType<IConfigureableAttribute<INameConfigurator>>())
            {
                attrubute.Apply(n);
            }
            return n.CreateNameList(declaringName);
        }
    }
}