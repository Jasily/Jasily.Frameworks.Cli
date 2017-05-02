using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jasily.Frameworks.Cli.Converters
{
    public interface IValueConverter
    {
        /// <summary>
        /// convert mulit value to array object.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        object Convert(IEnumerable<string> values);

        /// <summary>
        /// convert value to type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object Convert(string value);
    }

    public interface IValueConverter<out T> : IValueConverter
    {
        
    }
}
