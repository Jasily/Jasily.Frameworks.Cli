namespace Jasily.Frameworks.Cli.Converters
{
    internal class SingleConverter : StringConverter<float>
    {
        protected override float Convert(string value)
        {
            return float.Parse(value);
        }
    }
}
