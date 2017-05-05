using System;
using System.Collections.Generic;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal class ParameterConfigurator : IParameterConfigurator
    {
        public List<ICondition> Conditions { get; } = new List<ICondition>();

        public void AddCondition(ICondition condition)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            this.Conditions.Add(condition);
        }
    }
}