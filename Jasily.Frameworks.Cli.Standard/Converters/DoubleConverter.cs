namespace Jasily.Frameworks.Cli.Converters
{
    internal class DoubleConverter : StringConverter<double>
    {
        public override double Convert(string value)
        {
            return double.Parse(value);
        }
    }
}
