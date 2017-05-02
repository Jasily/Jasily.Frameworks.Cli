namespace Jasily.Frameworks.Cli.Converters
{
    public class SByteConverter : BaseConverter<sbyte>
    {
        protected override sbyte Convert(string value)
        {
            return sbyte.Parse(value);
        }
    }
}