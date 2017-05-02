namespace Jasily.Frameworks.Cli.Converters
{
    internal class DoubleConverter : BaseConverter<double>
    {
        protected override double Convert(string value)
        {
            return double.Parse(value);
        }
    }
}
