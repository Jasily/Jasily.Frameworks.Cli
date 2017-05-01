using System;
using System.Linq;
using System.Collections.Generic;
using Jasily.Frameworks.Cli.Core;

namespace Jasily.Frameworks.Cli.Core
{
    public class ArgumentParser : IArgumentParser
    {
        private Queue<ArgumentValue> requires;
        private IArgumentList arguments;
        private IReadOnlyList<ArgumentValue> valueList;

        private ArgumentValue TryGetByName(string name)
        {
            return this.valueList.SingleOrDefault(z => z.Parameter.IsMatchName(name));
        }

        private string Value(string value)
        {
            if (value.StartsWith("\\"))
            {
                value = value.Substring(1);
            }

            return value;
        }

        private void HandleNameValuePair(string name)
        {
            throw new NotImplementedException();

            (string n, string v) TrySplitName()
            {
                if (name.Contains('='))
                {
                    var g = name.Split(new [] { '=' }, 2);
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

            if (TryGetByName(n) is ArgumentValue slot)
            {
                this.arguments.UseOne();

                if (v == null && this.arguments.TryGetNextArgument(out var next) && !next.StartsWith("-"))
                {
                    this.arguments.UseOne();
                    slot.AddValue(next);
                }
                else
                {
                    slot.AddValue("");
                }
            }
        }

        private void HandleFlags(string flags)
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
                    return;
                }
            }
            avs.ForEach(z => z.AddValue(""));
        }

        private void HandleValue(string value)
        {
            if (this.requires.Count > 0)
            {
                var val = this.requires.Peek();
                if (val.IsAcceptValue())
                {
                    val.AddValue(value);
                    this.arguments.UseOne();
                }
                if (!val.IsAcceptValue())
                {
                    this.requires.Dequeue();
                }
            }
        }

        private void ParseCore()
        {
            while (this.arguments.TryGetNextArgument(out var value))
            {
                var cur = this.arguments.UsedArgvCount;

                if (value.StartsWith("--"))
                {
                    this.HandleNameValuePair(value.Substring(2));
                }
                else if (value.StartsWith("-"))
                {
                    this.HandleFlags(value.Substring(1));
                }
                else
                {
                    this.HandleValue(this.Value(value));
                }

                if (cur == this.arguments.UsedArgvCount) return;
            }
        }

        public void Parse(IArgumentList args, IReadOnlyList<ArgumentValue> values)
        {
            if (values.Count == 0) return;

            // set var
            this.arguments = args;
            this.valueList = values;
            this.requires = new Queue<ArgumentValue>(values.Where(z => !z.Parameter.ParameterInfo.HasDefaultValue));

            // start
            this.ParseCore();
        }
    }
}
