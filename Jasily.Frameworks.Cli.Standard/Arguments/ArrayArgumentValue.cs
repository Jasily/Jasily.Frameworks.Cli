using Jasily.Frameworks.Cli.Configurations;

namespace Jasily.Frameworks.Cli.Arguments
{
    internal class ArrayArgumentValue : ArgumentValue
    {
        public ArrayArgumentValue(IParameterConfiguration parameterConfiguration)
            : base(parameterConfiguration)
        {
        }

        public override bool IsSetedValue() => true;

        public override bool IsAcceptValue() => true;

        protected override object ConvertValue()
        {
            return this.ParameterConfiguration.ValueConverter.Convert(this.Values);
        }
    }
}