namespace Jasily.Frameworks.Cli.Converters
{
    internal class UInt32Converter : StringConverter<uint>
    {
        protected override uint Convert(string value)
        {
            return uint.Parse(value);
        }
    }
}