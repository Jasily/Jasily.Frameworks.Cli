using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Jasily.Frameworks.Cli.Core;
using Jasily.Frameworks.Cli.Exceptions;
using JetBrains.Annotations;

namespace Jasily.Frameworks.Cli.Core
{
    public class ArgumentParser : IArgumentParser
    {
        public void Parse(IArgumentList arguments, IReadOnlyList<ArgumentValue> valueList)
        {
            if (valueList.Count == 0) return;
            
            var requires = new Queue<ArgumentValue>(valueList.Where(z => !z.ParameterProperties.IsOptional));
            
            ArgumentValue TryGetByName(string name)
            {
                return valueList.SingleOrDefault(z => z.IsMatch(name));
            }

            string UnescapeValue(string value)
            {
                if (value.StartsWith("\\"))
                {
                    value = value.Substring(1);
                }

                return value;
            }

            bool OnNameValuePair(string name)
            {
                (string n, string v) TrySplitName()
                {
                    if (name.Contains('='))
                    {
                        var g = name.Split(new[] { '=' }, 2);
                        return (g[0], g[1]);
                    }
                    else if (name.Contains(':'))
                    {
                        var g = name.Split(new[] { ':' }, 2);
                        return (g[0], g[1]);
                    }
                    else
                    {
                        return (name, null);
                    }
                }

                var (n, v) = TrySplitName();

                if (TryGetByName(n) is ArgumentValue av)
                {
                    arguments.UseOne();

                    if (v == null &&
                        arguments.TryGetNextArgument(out var next) &&
                        !next.StartsWith("-"))
                    {
                        arguments.UseOne();
                        av.AddValue(UnescapeValue(next));
                    }
                    else
                    {
                        av.AddValue(string.Empty);
                    }
                    return true;
                }

                return false;
            }

            bool OnSingleCharFlags(string flags)
            {
                var avs = new List<ArgumentValue>();
                foreach (var ch in flags)
                {
                    if (TryGetByName(ch.ToString()) is ArgumentValue av)
                    {
                        avs.Add(av);
                    }
                    else
                    {
                        return arguments.UnknownArguments<bool>();
                    }
                }
                avs.ForEach(z => z.AddValue(string.Empty));
                arguments.UseOne();
                return true;
            }

            bool OnValueOnly(string value)
            {
                while (requires.Count > 0)
                {
                    var val = requires.Peek();
                    if (val.IsAcceptValue())
                    {
                        val.AddValue(value);
                        arguments.UseOne();
                        return true;
                    }
                    if (!val.IsAcceptValue())
                    {
                        requires.Dequeue();
                    }
                }
                return false;
            }

            // start
            while (arguments.TryGetNextArgument(out var value))
            {
                var cur = arguments.UsedArgvCount;

                if (value.StartsWith("--"))
                {
                    if (!OnNameValuePair(value.Substring(2))) return;
                }
                else if (value.StartsWith("-"))
                {
                    if (!OnSingleCharFlags(value.Substring(1))) return;
                }
                else
                {
                    if (!OnValueOnly(UnescapeValue(value))) return;
                }

                Debug.Assert(cur != arguments.UsedArgvCount);
            }
        }
    }
}
