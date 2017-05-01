namespace Jasily.Frameworks.Cli.Converters
{
    internal class BooleanConverter : StringConverter<bool>
    {
        public override bool Convert(string value)
        {
            return bool.Parse(value);
        }
    }
}
