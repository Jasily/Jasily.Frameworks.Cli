using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jasily.Frameworks.Cli.IO
{
    internal class ConsoleOutput : IOutput
    {
        private void SetColor(OutputLevel level)
        {
            switch (level)
            {
                case OutputLevel.Normal:
                    break;

                case OutputLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case OutputLevel.Usage:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                default:
                    break;
            }
        }

        public void Write(OutputLevel level, string value)
        {
            this.SetColor(level);
            Console.Write(value);
            Console.ResetColor();
        }

        public void WriteLine(OutputLevel level, string value)
        {
            this.SetColor(level);
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
