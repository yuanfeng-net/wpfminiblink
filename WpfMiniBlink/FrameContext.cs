using System;

namespace Miniblink
{
    public class FrameContext
    {
        public IntPtr Id { get; }
        public bool IsMain { get; }
        public string Url { get; }
        public bool IsRemote { get; }
        private IntPtr _mb;

        internal FrameContext(IMiniblink miniblink, IntPtr frameId)
        {
            _mb = miniblink.MiniblinkHandle;
            Id = frameId;
            IsMain = MBApi.wkeIsMainFrame(_mb, frameId);
            Url = MBApi.wkeGetFrameUrl(_mb, frameId).ToUTF8String();
            IsRemote = MBApi.wkeIsWebRemoteFrame(_mb, frameId);
        }

        public object RunJs(string script)
        {
            var es = MBApi.wkeGetGlobalExecByFrame(_mb, Id);
            return MBApi.jsEvalExW(es, script, true).ToValue(es);
        }

        public void InsertCss(string cssText)
        {
            MBApi.wkeInsertCSSByFrame(_mb, Id, cssText);
        }
    }
}
