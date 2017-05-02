namespace Jasily.Frameworks.Cli.Converters
{
    public class DecimalConverter : BaseConverter<decimal>
    {
        protected override decimal Convert(string value)
        {
            return decimal.Parse(value);
        }
    }
}