using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Miniblink.LocalHttp
{
    public class NetApiAfter : EventArgs
    {
        public object Instance { get; internal set; }
        public MethodInfo Method { get; internal set; }
        public NetApiRequest Request { get; }
        public string Result { get; internal set; }

        internal NetApiAfter(NetApiRequest request)
        {
            Request = request;
        }
    }
}
