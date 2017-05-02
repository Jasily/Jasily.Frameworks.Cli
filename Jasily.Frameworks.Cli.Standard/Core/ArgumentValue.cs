using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Exceptions;

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

        /// <summary>
        /// verify whether is value is ok.
        /// </summary>
        internal void Verify()
        {
            if (this._parameterConfiguration.IsArray)
            {
                if (this._parameterConfiguration.ArrayMinLength > 0)
                {
                    if (this._parameterConfiguration.ArrayMinLength > this.Values.Count)
                    {
                        throw InvalidArgumentException.Build(this, $"count >= {this.ParameterProperties.ArrayMinLength}");
                    }
                }

                if (this._parameterConfiguration.ArrayMaxLength > 0)
                {
                    if (this._parameterConfiguration.ArrayMaxLength < this.Values.Count)
                    {
                        throw InvalidArgumentException.Build(this, $"count <= {this.ParameterProperties.ArrayMaxLength}");
                    }
                }
            }
            else if (this.Values.Count > 1)
            {
                throw new ConvertException("too many arguments.");
            }
        }

        internal object GetValue()
        {
            return this._parameterConfiguration.IsArray
                ? this._parameterConfiguration.ValueConverter.Convert(this.Values)
                : this._parameterConfiguration.ValueConverter.Convert(this.Values.Single());
        }
    }
}
