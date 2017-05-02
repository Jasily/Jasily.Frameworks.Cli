namespace Jasily.Frameworks.Cli.Converters
{
    internal class BooleanConverter : BaseConverter<bool>
    {
        protected override bool Convert(string value)
        {
            return bool.Parse(value);
        }
    }
}
