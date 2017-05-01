using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.Frameworks.Cli.Core
{
    internal class InstanceContainer<T>
    {
        private T value;
        private bool isSet;

        public T Instance { get; set; }

        public T GetOrThrow()
        {
            if (this.isSet) return this.value;
            throw new InvalidOperationException();
        }

        public void Set(T value)
        {
            this.value = value;
            this.isSet = true;
        }
    }
}
