using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jasily.Frameworks.Cli.Commands;

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
    }
}
