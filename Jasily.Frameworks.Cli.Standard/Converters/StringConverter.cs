namespace Jasily.Frameworks.Cli.Converters
{
    public class StringConverter : BaseConverter<string>
    {
        protected override string Convert(string value)
        {
            return value;
        }
    }
}