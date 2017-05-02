using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli
{
    /// <summary>
    /// cli engine interface.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// fire the <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        Executor Fire(object instance);
    }
}
