using System;

namespace Miniblink
{
    public delegate object TempNetFunc(object[] param);

    public delegate object NetFuncDelegate(NetFuncContext context);

    public class NetFuncContext
    {
        public string Name { get; internal set; }
        public object State { get; internal set; }
        public object[] Paramters { get; internal set; }
    }

    public class NetFunc
    {
        public string Name { get; }
        private NetFuncDelegate _func { get; }
        private object _state { get; }

        public NetFunc(string name, NetFuncDelegate func, object state = null)
        {
            Name = name;
            _func = func;
            _state = state;
        }

        internal object OnFunc(object[] param)
        {
            return _func(new NetFuncContext
            {
                Name = Name,
                State = _state,
                Paramters = param ?? new object[0]
            });
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class NetFuncAttribute : Attribute
    {
        public string Name { get; }

        public NetFuncAttribute(string functionName = null)
        {
            Name = functionName;
        }
    }
}
