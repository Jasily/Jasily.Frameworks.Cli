using System.Collections.Generic;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.IO
{
    public interface IUsageDrawer
    {
        void DrawRouter(ICommandRouter router);

        void DrawParameter(ICommandProperties command, IReadOnlyList<ParameterInfoDescriptor> parameters);
    }
}
