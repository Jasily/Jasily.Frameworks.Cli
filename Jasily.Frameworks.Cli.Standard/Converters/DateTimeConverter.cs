using System;

namespace Jasily.Frameworks.Cli.Converters
{
    public class DateTimeConverter : BaseConverter<DateTime>
    {
        protected override DateTime Convert(string value)
        {
            return DateTime.Parse(value);
        }
    }
}