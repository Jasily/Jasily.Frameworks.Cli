using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Core;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Arguments
{
    internal abstract class ArgumentValue : IArgumentValue
    {
        private readonly List<string> _values = new List<string>();
        protected readonly IParameterConfiguration ParameterConfiguration;

        protected ArgumentValue(IParameterConfiguration parameterConfiguration)
        {
            this.ParameterConfiguration = parameterConfiguration;
            this.Values = new ReadOnlyCollection<string>(this._values);
            this.ParameterName = this.ParameterConfiguration.ParameterInfo.Name;
        }

        /// <summary>
        /// Parameter Declaring Name
        /// </summary>
        public string ParameterName { get; }

        public IReadOnlyList<string> Values { get; }

        public IParameterProperties ParameterProperties => this.ParameterConfiguration;

        public bool IsMatch(string name)
        {
            return this.ParameterConfiguration.IsMatchName(name);
        }

        public void AddValue(string value)
        {
            this._values.Add(value);
        }

        public virtual bool IsSetedValue() => this._values.Count > 0;

        public virtual bool IsAcceptValue() => this._values.Count == 0;

        public object GetValue()
        {
            var value = this.ConvertValue();
            try
            {
                this.ParameterConfiguration.Check(value);
            }
            catch (InvalidArgumentException e)
            {
                return this.InvalidArgument<object>(e.Message);
            }
            return value;
        }

        protected abstract object ConvertValue();

        public static IArgumentValue Create(IParameterConfiguration parameter)
        {
            if (parameter.IsArray)
            {
                return new ArrayArgumentValue(parameter);
            }

            return new DefaultArgumentValue(parameter);
        }
    }
}
