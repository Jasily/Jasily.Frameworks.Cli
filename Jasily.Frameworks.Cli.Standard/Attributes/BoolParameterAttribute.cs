using System;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// this attribute only effect bool type parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class BoolParameterAttribute : Attribute
    {
        public bool Value { get; set; }

        public string[] Keywords { get; set; }
    }
}
