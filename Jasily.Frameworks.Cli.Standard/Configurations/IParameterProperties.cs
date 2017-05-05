using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Configurations
{
    /// <summary>
    /// properties of method parameter.
    /// </summary>
    public interface IParameterProperties : IBaseProperties
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
        /// is parameter type is array (var length).
        /// </summary>
        bool IsArray { get; }

        /// <summary>
        /// element type of array.
        /// </summary>
        [CanBeNull]
        Type ArrayElementType { get; }

        IReadOnlyList<string> Conditions { get; }
    }
}