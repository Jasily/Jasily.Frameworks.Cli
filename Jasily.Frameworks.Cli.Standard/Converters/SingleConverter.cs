namespace Jasily.Frameworks.Cli.Converters
{
    internal class SingleConverter : BaseConverter<float>
    {
        protected override float Convert(string value)
        {
            return float.Parse(value);
        }
    }
}
