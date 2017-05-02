namespace Jasily.Frameworks.Cli.Converters
{
    internal class UInt64Converter : BaseConverter<ulong>
    {
        protected override ulong Convert(string value)
        {
            return ulong.Parse(value);
        }
    }
}