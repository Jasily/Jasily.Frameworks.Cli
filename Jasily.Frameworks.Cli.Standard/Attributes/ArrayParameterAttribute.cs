﻿using System;

namespace Jasily.Frameworks.Cli.Attributes
{
    /// <summary>
    /// this attribute only effect array typed parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ArrayParameterAttribute : Attribute
    {
        public int? MinCount { get; set; }

        public int? MaxCount { get; set; }
    }
}
