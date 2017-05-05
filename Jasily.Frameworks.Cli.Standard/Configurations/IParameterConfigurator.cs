using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    public interface IParameterConfigurator
    {
        void AddCondition([NotNull] ICondition condition);
    }
}