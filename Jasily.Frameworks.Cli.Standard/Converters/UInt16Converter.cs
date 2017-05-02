namespace Jasily.Frameworks.Cli.Converters
{
    public class UInt16Converter : BaseConverter<ushort>
    {
        protected override ushort Convert(string value)
        {
            return ushort.Parse(value);
        }
    }
}