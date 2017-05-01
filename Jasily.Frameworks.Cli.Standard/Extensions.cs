using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Jasily.Frameworks.Cli
{
    public static class Extensions
    {
        public static bool TryGetNextArgument(this IArgumentList args, out string value)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            if (args.Argv.Count == args.UsedArgvCount)
            {
                value = null;
                return false;
            }
            else
            {
                value = args.Argv[args.UsedArgvCount];
                return true;
            }
        }

        /// <summary>
        /// use one arg.
        /// </summary>
        /// <param name="args"></param>
        public static void UseOne(this IArgumentList args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            args.Use(1);
        }

        public static bool IsAllUsed(this IArgumentList args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            return args.Argv.Count == args.UsedArgvCount;
        }

        public static string[] GetUnusedArguments(this IArgumentList args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            return args.Argv.Skip(args.UsedArgvCount).ToArray();
        }
    }
}
