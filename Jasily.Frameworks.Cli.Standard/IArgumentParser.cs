using System.Collections.Generic;
using System.Text;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli
{
    /// <summary>
    /// you can impl your own argument parser.
    /// </summary>
    public interface IArgumentParser
    {
        void Parse(IArgumentList args, IReadOnlyList<ArgumentValue> values);
    }
}
