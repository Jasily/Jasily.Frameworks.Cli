using System.IO;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Converters
{
    internal class DirectoryInfoConverter : StringConverter<DirectoryInfo>
    {
        public override DirectoryInfo Convert(string value)
        {
            if (!Directory.Exists(value)) throw new MessageException("file is not exists.");

            return new DirectoryInfo(value);
        }
    }
}
