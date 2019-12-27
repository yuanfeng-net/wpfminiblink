using System;
using System.Threading.Tasks;

namespace Miniblink
{
    public class NetJob
    {
        public IntPtr Handle { get; }
        public IntPtr WebView { get; }
        public object State { get; private set; }
        public byte[] Data { get; set; }
        public string ResponseContentType { get; set; }
		public string Url { get; internal set; }

        internal bool IsAsync;
        internal LoadUrlBeginEventArgs BeginArgs;
        private Action<NetJob> _completed;

	    internal NetJob(IntPtr webview, IntPtr job, Action<NetJob> completed = null)
	    {
		    Handle = job;
		    WebView = webview;
		    _completed = completed;
	    }

	    public void Wait(Action<NetJob> callback, object state = null)
	    {
			if(BeginArgs.Ended)
				return;

		    IsAsync = true;
		    State = state;
		    MBApi.wkeNetHoldJobToAsynCommit(Handle);

		    Task.Factory.StartNew(() =>
		    {
			    try
			    {
				    callback(this);
			    }
			    finally
			    {
				    _completed?.Invoke(this);
			    }
		    });
	    }

	    public void WatchLoadUrlEnd(Action<LoadUrlEndArgs> callback, object state = null)
	    {
		    BeginArgs.WatchLoadUrlEnd(callback);
	    }
    }
}
