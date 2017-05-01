namespace Jasily.Frameworks.Cli.Converters
{
    internal class FloatConverter : StringConverter<float>
    {
        public override float Convert(string value)
        {
            return float.Parse(value);
        }
    }
}
