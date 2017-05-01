using System;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// declare the method is a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandMethodAttribute : BaseCommandAttribute
    {
    }
}
