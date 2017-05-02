namespace Jasily.Frameworks.Cli.Converters
{
    public class Int16Converter : BaseConverter<short>
    {
        protected override short Convert(string value)
        {
            return short.Parse(value);
        }
    }
}