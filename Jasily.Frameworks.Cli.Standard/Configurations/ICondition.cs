namespace Jasily.Frameworks.Cli.Configurations
{
    public interface ICondition
    {
        void Check(object value);
    }
}