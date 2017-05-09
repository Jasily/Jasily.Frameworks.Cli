using System;
using System.IO;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Converters
{
    internal class DirectoryInfoConverter : BaseConverter<DirectoryInfo>
    {
        protected override DirectoryInfo Convert(string value)
        {
            if (!Directory.Exists(value)) throw new InvalidOperationException("file is not exists.");

            return new DirectoryInfo(value);
        }
    }
}
