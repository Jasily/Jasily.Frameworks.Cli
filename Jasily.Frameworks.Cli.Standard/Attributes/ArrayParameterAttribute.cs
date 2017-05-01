using System;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// this attribute only effect array typed parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ArrayParameterAttribute : Attribute
    {
        /// <summary>
        /// valid value is (0,)
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// valid value is (0,)
        /// </summary>
        public int MaxLength { get; set; }
    }
}
