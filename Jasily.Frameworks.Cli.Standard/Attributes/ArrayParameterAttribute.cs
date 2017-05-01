using System;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// this attribute only effect array typed parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ArrayParameterAttribute : Attribute
    {
        public uint? MinLength { get; set; }

        public uint? MaxLength { get; set; }
    }
}
