using System;
using System.Collections.Generic;
using System.Reflection;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.DependencyInjection.MemberInjection;
using Jasily.Frameworks.Cli.Attributes;

namespace Jasily.Frameworks.Cli.Commands
{
    internal class PropertyCommand<TClass> : RealizedCommand<TClass>
    {
        public PropertyCommand(IServiceProvider serviceProvider, PropertyInfo property)
            : base(serviceProvider, property.GetMethod)
        {
            this.AddName(property.Name);
        }

        public CommandPropertyAttribute Attribute { get; }

        public override object Invoke(TClass instance, IServiceProvider serviceProvider, OverrideArguments args)
        {
            return this.Method.Invoke(instance, new object[0]);
        }
    }
}
