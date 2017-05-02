namespace Jasily.Frameworks.Cli.Converters
{
    internal class Int64Converter : BaseConverter<long>
    {
        protected override long Convert(string value)
        {
            return long.Parse(value);
        }
    }
}
