using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniblink
{
    public class WKEException : Exception
    {
        public WKEException(string message = null) : base(message)
        {

        }
    }

    public class WKECreateException : WKEException
    {
        
    }

    public class WKEFunctionNotFondException : WKEException
    {
        public string FunctionName { get; }

        public WKEFunctionNotFondException(string functionName)
        {
            FunctionName = functionName;
        }
    }
}
