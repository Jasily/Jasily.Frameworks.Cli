using System.IO;
using System.Text;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Converters
{
    internal class FileInfoConverter : StringConverter<FileInfo>
    {
        public override FileInfo Convert(string value)
        {
            if (!File.Exists(value)) throw new MessageException("file is not exists.");

            return new FileInfo(value);
        }
    }
}
