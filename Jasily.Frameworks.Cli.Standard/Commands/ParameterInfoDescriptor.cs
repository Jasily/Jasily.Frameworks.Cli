using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Jasily.Frameworks.Cli.Attributes;

namespace Jasily.Frameworks.Cli.Commands
{
    public class ParameterInfoDescriptor
    {
        private readonly StringComparer stringComparer;
        private readonly HashSet<string> nameSet;
        private readonly Dictionary<string, string> boolMap;

        public ParameterInfoDescriptor(ParameterInfo parameter, StringComparer comaprer)
        {
            this.ParameterInfo = parameter;
            this.stringComparer = comaprer;

            var names = new List<string> { parameter.Name };
            if (parameter.GetCustomAttribute<CommandParameterAttribute>() is CommandParameterAttribute attr)
            {
                if (attr.Names != null)
                {
                    foreach (var item in attr.Names)
                    {
                        names.Add(item);
                    }
                }

                this.IsAutoPadding = attr.IsAutoPadding;
            }
            this.Names = new ReadOnlyCollection<string>(names);
            this.nameSet = new HashSet<string>(names);

            if (this.ParameterInfo.ParameterType == typeof(bool))
            {
                this.boolMap = new Dictionary<string, string>(comaprer);
                foreach (var boolAttr in this.ParameterInfo.GetCustomAttributes<BoolParameterAttribute>())
                {
                    if (boolAttr.Keywords != null)
                    {
                        foreach (var item in boolAttr.Keywords.Where(z => z != null))
                        {
                            this.boolMap[item] = boolAttr.Value.ToString();
                        }
                    }
                }
            }
            else if (this.ParameterInfo.ParameterType.IsArray)
            {
                this.IsArray = true;

                if (this.ParameterInfo.GetCustomAttribute<ArrayParameterAttribute>() is
                    ArrayParameterAttribute arrayAttr)
                {
                    this.ArrayMinCount = arrayAttr.MinCount;
                    this.ArrayMaxCount = arrayAttr.MaxCount;
                }
            }
        }

        public ParameterInfo ParameterInfo { get; }

        public IReadOnlyList<string> Names { get; }

        public bool IsAutoPadding { get; }

        public bool IsMatchName(string name)
        {
            return this.nameSet.Contains(name);
        }

        internal string ConvertValueInternal(string value)
        {
            if (this.boolMap != null && this.boolMap.TryGetValue(value, out var val) == true)
            {
                return val;
            }

            return value;
        }

        #region array typed parameter

        public bool IsArray { get; }

        public int? ArrayMinCount { get; }

        public int? ArrayMaxCount { get; }

        #endregion
    }
}
