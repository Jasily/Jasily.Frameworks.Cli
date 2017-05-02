using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Frameworks.Cli.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Jasily.Core;
using Jasily.Frameworks.Cli.Configurations;
using Jasily.Frameworks.Cli.Exceptions;

namespace Jasily.Frameworks.Cli.Commands
{
    internal class ClassCommand<T> : ICommand
    {
        private readonly TypeConfiguration<T> _configuration;

        public ClassCommand(TypeConfiguration<T> configuration)
        {
            this._configuration = configuration;
        }

        public object Invoke(IServiceProvider serviceProvider, object instance)
        {
            return instance;
        }

        public ICommandProperties Properties => this._configuration;
    }
}
