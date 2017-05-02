namespace Jasily.Frameworks.Cli.Converters
{
    internal class BooleanConverter : StringConverter<bool>
    {
        protected override bool Convert(string value)
        {
            return bool.Parse(value);
        }
    }
}
