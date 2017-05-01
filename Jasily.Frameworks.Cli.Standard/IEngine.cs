namespace Jasily.Frameworks.Cli
{
    /// <summary>
    /// cli engine interface.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// execute in scope session.
        /// </summary>
        /// <param name="argv"></param>
        object Execute(string[] argv);
    }
}
