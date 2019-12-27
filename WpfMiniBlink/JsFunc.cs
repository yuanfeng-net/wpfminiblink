using System;
using System.Linq;
using WpfMiniBlink;

namespace Miniblink
{
    public delegate object JsFunc(params object[] param);

    public class JsFuncWapper
    {
        private string _name;
        private IntPtr _mb;

        internal JsFuncWapper(long jsvalue, IntPtr es)
        {
            _name = "func" + Guid.NewGuid().ToString().Replace("-", "");

            _mb = MBApi.jsGetWebView(es);

            MBApi.jsSetGlobal(es, _name, jsvalue);
        }

        public object Call(params object[] param)
        {
            object result = null;

			MiniblinkBrowser.InvokeBro.UIInvoke(() =>
			{
				var es = MBApi.wkeGlobalExec(_mb);
				var value = MBApi.jsGetGlobal(es, _name);
				var jsps = param.Select(i => i.ToJsValue(es)).ToArray();
				result = MBApi.jsCall(es, value, MBApi.jsUndefined(), jsps, jsps.Length).ToValue(es);
				MBApi.jsSetGlobal(es, _name, MBApi.jsUndefined());
			});
			
            return result;
        }
    }
}
