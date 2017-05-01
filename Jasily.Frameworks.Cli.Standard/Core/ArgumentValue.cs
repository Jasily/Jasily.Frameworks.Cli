using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jasily.Frameworks.Cli.Commands;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Core
{
    public class ArgumentValue
    {
        private readonly List<string> values = new List<string>();

        internal ArgumentValue(ParameterInfoDescriptor parameter)
        {
            this.Parameter = parameter;
            this.Values = new ReadOnlyCollection<string>(this.values);
        }

        /// <summary>
        /// 
        /// </summary>
        public ParameterInfoDescriptor Parameter { get; }

        public IReadOnlyList<string> Values { get; }

        public void AddValue(string value)
        {
            value = this.Parameter.ConvertValueInternal(value);
            this.values.Add(value);
        }

        public bool IsSetedValue()
        {
            return this.Parameter.IsArray || this.values.Count > 0;
        }

        public bool IsAcceptValue()
        {
            return this.Parameter.IsArray || this.values.Count == 0;
        }

        /// <summary>
        /// verify whether is value is ok.
        /// </summary>
        internal void Verify()
        {
            if (this.Parameter.IsArray)
            {
                if (this.Parameter.ArrayMinLength != null)
                {
                    if (this.Parameter.ArrayMinLength > this.Values.Count)
                    {
                        var msg = $"count >= {this.Parameter.ArrayMinLength}";
                        throw InvalidArgumentException.Build(this, msg);
                    }
                }

                if (this.Parameter.ArrayMaxLength != null)
                {
                    if (this.Parameter.ArrayMaxLength < this.Values.Count)
                    {
                        var msg = $"count <= {this.Parameter.ArrayMaxLength}";
                        throw InvalidArgumentException.Build(this, msg);
                    }
                }
            }
        }
    }
}
