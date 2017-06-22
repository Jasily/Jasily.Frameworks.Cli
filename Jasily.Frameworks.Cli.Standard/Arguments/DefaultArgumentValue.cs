using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Arguments
{
    internal class DefaultArgumentValue : ArgumentValue
    {
        public DefaultArgumentValue(IParameterConfiguration parameterConfiguration)
            : base(parameterConfiguration)
        {
        }

        protected override object ConvertValue()
        {
            switch (this.Values.Count)
            {
                case 0:
                    if (this.ParameterConfiguration.ParameterInfo.HasDefaultValue)
                    {
                        return this.ParameterConfiguration.ParameterInfo.DefaultValue;
                    }
                    return ExceptionThrower.UnResolveArgument<object>(this);

                case 1:
                    return this.ParameterConfiguration.ValueConverter.Convert(this.Values[0]);

                default:
                    throw new ConvertException("too many arguments.");
            }
        }
    }
}