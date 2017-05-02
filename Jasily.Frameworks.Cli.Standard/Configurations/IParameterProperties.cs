using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    /// <summary>
    /// properties of method parameter.
    /// </summary>
    public interface IParameterProperties
    {
        /// <summary>
        /// 
        /// </summary>
        ParameterInfo ParameterInfo { get; }

        /// <summary>
        /// is parameter should resolve by engine.
        /// </summary>
        bool IsResolveByEngine { get; }

        /// <summary>
        /// is parameter has default value.
        /// </summary>
        bool IsOptional { get; }

        /// <summary>
        /// names of parameter.
        /// </summary>
        [NotNull]
        IReadOnlyList<string> Names { get; }

        /// <summary>
        /// is parameter type is array (var length).
        /// </summary>
        bool IsArray { get; }

        /// <summary>
        /// min length of array.
        /// </summary>
        int ArrayMinLength { get; }

        /// <summary>
        /// max length of array.
        /// </summary>
        int ArrayMaxLength { get; }

        /// <summary>
        /// element type of array.
        /// </summary>
        [CanBeNull]
        Type ArrayElementType { get; }
    }
}