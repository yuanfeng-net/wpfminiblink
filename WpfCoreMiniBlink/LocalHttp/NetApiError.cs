using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Miniblink.LocalHttp
{
    public class NetApiError : EventArgs
    {
        public object Instance { get; internal set; }
        public MethodInfo Method { get; internal set; }
        public NetApiRequest Request { get; }
        public Exception Error { get; internal set; }
        public string Result { get; set; }

        internal NetApiError(NetApiRequest request)
        {
            Request = request;
        }
    }
}
