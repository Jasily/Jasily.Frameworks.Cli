using System;

namespace Jasily.Frameworks.Cli.Attributes.Parameters
{
    public sealed class ArrayLengthAttribute : ParameterConditionAttribute
    {
        /// <summary>
        /// valid value is (0,)
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// valid value is (0,)
        /// </summary>
        public int MaxLength { get; set; }

        public override void Check(object value)
        {
            if (value is Array array)
            {
                if (this.MinLength > 0 && array.Length < this.MinLength)
                {
                    this.InvalidArgument($"count >= {this.MinLength}");
                }

                if (this.MaxLength > 0 && array.Length > this.MaxLength)
                {
                    this.InvalidArgument($"count <= {this.MaxLength}");
                }
            }
        }

        public override string ToString()
        {
            if (this.MinLength > 0)
            {
                if (this.MaxLength > 0)
                {
                    return $"{this.MinLength}<COUNT<{this.MaxLength}";
                }
                else
                {
                    return $"{this.MinLength}<COUNT";
                }
            }
            else if (this.MaxLength > 0)
            {
                return $"COUNT<{this.MaxLength}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
