using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Miniblink.LocalHttp
{
    public class NetApiBefore : EventArgs
    {
        public object Instance { get; set; }
        public MethodInfo Method { get; set; }
        public NetApiRequest Request { get; }
        public string Result { get; set; }

        internal NetApiBefore(NetApiRequest request)
        {
            Request = request;
        }
    }
}
