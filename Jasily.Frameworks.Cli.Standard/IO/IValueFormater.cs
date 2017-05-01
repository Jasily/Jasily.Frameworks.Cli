using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli.IO
{
    public interface IValueFormater
    {
        string Format(object obj);
    }
}
