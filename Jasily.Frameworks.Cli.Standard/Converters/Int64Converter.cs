namespace Jasily.Frameworks.Cli.Converters
{
    internal class Int64Converter : StringConverter<long>
    {
        public override long Convert(string value)
        {
            return long.Parse(value);
        }
    }
}
