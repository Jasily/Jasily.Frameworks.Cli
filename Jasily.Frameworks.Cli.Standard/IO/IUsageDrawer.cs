using System.Collections.Generic;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.IO
{
    /// <summary>
    /// usage drawer.
    /// </summary>
    public interface IUsageDrawer
    {
        /// <summary>
        /// draw router.
        /// </summary>
        /// <param name="router"></param>
        void DrawRouter(ICommandRouter router);

        /// <summary>
        /// draw router.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        void DrawParameter(ICommandProperties command, IReadOnlyList<IParameterProperties> parameters);
    }
}
