namespace Jasily.Frameworks.Cli.Converters
{
    public class ByteConverter : BaseConverter<byte>
    {
        protected override byte Convert(string value)
        {
            return byte.Parse(value);
        }
    }
}