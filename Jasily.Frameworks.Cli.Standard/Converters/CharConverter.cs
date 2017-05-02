namespace Jasily.Frameworks.Cli.Converters
{
    public class CharConverter : BaseConverter<char>
    {
        protected override char Convert(string value)
        {
            return char.Parse(value);
        }
    }
}