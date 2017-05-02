namespace Jasily.Frameworks.Cli.Converters
{
    internal class Int32Converter : StringConverter<int>
    {
        protected override int Convert(string value)
        {
            return int.Parse(value);
        }
    }
}
