using System;

namespace Jasily.Frameworks.Cli.Configurations
{
    internal class ArrayParameterConfigurator : IArrayParameterConfigurator
    {
        private int _minLength;
        private int _maxLength;

        public int MinLength
        {
            get => this._minLength;
            set
            {
                if (value < 0) throw new InvalidOperationException();
                this._minLength = value;
            }
        }

        public int MaxLength
        {
            get => this._maxLength;
            set
            {
                if (value < 0) throw new InvalidOperationException();
                this._maxLength = value;
            }
        }
    }
}