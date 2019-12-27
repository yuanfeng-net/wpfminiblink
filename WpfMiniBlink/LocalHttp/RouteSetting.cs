using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Miniblink.LocalHttp
{
    public class RouteSetting
    {
        public Type ClassType { get; internal set; }
        public MethodInfo Method { get; internal set; }
        public RequestType Type { get; internal set; }
        public string Path { get; internal set; }
    }

    public enum RequestType
    {
        Unkonw,
        GET,
        POST
    }
}
