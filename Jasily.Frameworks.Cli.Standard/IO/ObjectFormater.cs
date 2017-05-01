namespace Jasily.Frameworks.Cli.IO
{
    public class ObjectFormater : IValueFormater
    {
        public string Format(object obj)
        {
            return obj.ToString();
        }
    }
}
