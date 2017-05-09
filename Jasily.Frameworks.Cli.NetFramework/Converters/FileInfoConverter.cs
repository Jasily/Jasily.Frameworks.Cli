using System;
using System.IO;
using System.Text;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Converters
{
    internal class FileInfoConverter : BaseConverter<FileInfo>
    {
        protected override FileInfo Convert(string value)
        {
            if (!File.Exists(value)) throw new InvalidOperationException("file is not exists.");

            return new FileInfo(value);
        }
    }
}
