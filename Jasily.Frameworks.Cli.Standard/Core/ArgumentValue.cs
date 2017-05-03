using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Core
{
    public class ArgumentValue
    {
        private readonly IParameterConfiguration _parameterConfiguration;
        private readonly List<string> _values = new List<string>();

        internal ArgumentValue(IParameterConfiguration parameterConfiguration)
        {
            this._parameterConfiguration = parameterConfiguration;
            this.Values = new ReadOnlyCollection<string>(this._values);
            this.ParameterName = this._parameterConfiguration.ParameterInfo.Name;
        }

        /// <summary>
        /// Parameter Declaring Name
        /// </summary>
        public string ParameterName { get; }

        public IReadOnlyList<string> Values { get; }

        public IParameterProperties ParameterProperties => this._parameterConfiguration;

        public bool IsMatch(string name)
        {
            return this._parameterConfiguration.IsMatchName(name);
        }

        public void AddValue(string value)
        {
            this._values.Add(value);
        }

        public bool IsSetedValue()
        {
            return this._parameterConfiguration.IsArray || this._values.Count > 0;
        }

        public bool IsAcceptValue()
        {
            return this._parameterConfiguration.IsArray || this._values.Count == 0;
        }

        internal object GetValue()
        {
            // array
            if (this._parameterConfiguration.IsArray)
            {
                if (this._parameterConfiguration.ArrayMinLength > 0)
                {
                    if (this._parameterConfiguration.ArrayMinLength > this.Values.Count)
                    {
                        return this.InvalidArgument<object>($"count >= {this.ParameterProperties.ArrayMinLength}");
                    }
                }

                if (this._parameterConfiguration.ArrayMaxLength > 0)
                {
                    if (this._parameterConfiguration.ArrayMaxLength < this.Values.Count)
                    {
                        return this.InvalidArgument<object>($"count <= {this.ParameterProperties.ArrayMaxLength}");
                    }
                }

                return this._parameterConfiguration.ValueConverter.Convert(this.Values);
            }

            // single
            switch (this.Values.Count)
            {
                case 0:
                    if (this._parameterConfiguration.ParameterInfo.HasDefaultValue)
                    {
                        return this._parameterConfiguration.ParameterInfo.DefaultValue;
                    }
                    return this.UnResolveArgument<object>();

                case 1:
                    return this._parameterConfiguration.ValueConverter.Convert(this.Values[0]);

                default:
                    throw new ConvertException("too many arguments.");
            }
        }
    }
}
