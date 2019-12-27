using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miniblink.LocalHttp
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiNameAttribute : Attribute
    {
        public string Name { get; }

        public ApiNameAttribute(string name)
        {
            Name = name;
        }
    }
}
