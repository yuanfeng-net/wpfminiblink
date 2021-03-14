using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Miniblink
{
    public class MiniblinkEventArgs : EventArgs
    {
        internal MiniblinkEventArgs()
        {
        }
    }

    public class UrlChangedEventArgs : MiniblinkEventArgs
    {
        public string Url { get; internal set; }
        public FrameContext Frame { get; internal set; }

        internal UrlChangedEventArgs()
        {
        }
    }

    public class NavigateEventArgs : MiniblinkEventArgs
    {
        public string Url { get; internal set; }
        public NavigateType Type { get; internal set; }
        public bool Cancel { get; set; }

        internal NavigateEventArgs()
        {
        }
    }

    public class DocumentReadyEventArgs : MiniblinkEventArgs
    {
        public FrameContext Frame { get; internal set; }

        internal DocumentReadyEventArgs()
        {
        }
    }

    public class ConsoleMessageEventArgs : MiniblinkEventArgs
    {
        public wkeConsoleLevel Level { get; internal set; }
        public string Message { get; internal set; }
        public string SourceName { get; internal set; }
        public int SourceLine { get; internal set; }
        public string StackTrace { get; internal set; }

        internal ConsoleMessageEventArgs()
        {
        }
    }

    public class NetResponseEventArgs : MiniblinkEventArgs
    {
        public string Url { get; internal set; }
        public IntPtr Job { get; internal set; }
        public bool Cancel { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; internal set; }

        internal NetResponseEventArgs()
        {
        }

        public string GetHeader(string name)
        {
            return MBApi.wkeNetGetHTTPHeaderFieldFromResponse(Job, name).ToUTF8String();
        }
    }

    public class LoadUrlBeginEventArgs : MiniblinkEventArgs
    {
        private static ConcurrentDictionary<long, LoadUrlBeginEventArgs> _args =
            new ConcurrentDictionary<long, LoadUrlBeginEventArgs>();

        public wkeRequestType RequestMethod { get; internal set; }
        public string Url { get; internal set; }
        public NetJob Job { get; internal set; }
        public byte[] Data { get; set; }
        public bool Cancel { get; set; }
        public bool IsLocalFile { get; internal set; }
        internal bool HookRequest { get; set; }
        private List<Tuple<Action<LoadUrlEndArgs>, object>> _loadUrlEnd;
        internal bool Ended;
        private PostBody _postBody;

        internal LoadUrlBeginEventArgs()
        {
            _loadUrlEnd = new List<Tuple<Action<LoadUrlEndArgs>, object>>();
        }

        public PostBody GetPostBody()
        {
            if (_postBody == null)
            {
                _postBody = new PostBody(Job.Handle);
            }

            return _postBody;
        }

        /// <summary>
        /// 请求头
        /// </summary>
        /// <returns></returns>
        public string GetAllHead()
        {
            var ptr = MBApi.wkeNetGetRawHttpHead(Job.Handle);
            StringBuilder sbhead = new StringBuilder();
            wkeSlist els ;
            again:
            els = (wkeSlist)Marshal.PtrToStructure(ptr, typeof(wkeSlist));

            string head = Marshal.PtrToStringAnsi(els.str);
            sbhead.AppendLine(head);
            if (els.next != IntPtr.Zero)
            {
                ptr = els.next;
                goto again;
            }

            return sbhead.ToString();
        }

        public void SetHeader(string name, string value)
        {
            MBApi.wkeNetSetHTTPHeaderField(Job.Handle, name, value);
        }

        public void WatchLoadUrlEnd(Action<LoadUrlEndArgs> callback, object state = null)
        {
            _loadUrlEnd.Add(new Tuple<Action<LoadUrlEndArgs>, object>(callback, state));

            if (HookRequest == false)
            {
                _args.TryAdd(Job.Handle.ToInt64(), this);

                HookRequest = true;
            }
        }

        internal LoadUrlEndArgs OnLoadUrlEnd(byte[] data)
        {
            Ended = true;

            if (HookRequest == false)
                return null;

            var e = new LoadUrlEndArgs
            {
                Data = data,
                Job = Job.Handle,
                RequestMethod = RequestMethod,
                Url = Url
            };
            _loadUrlEnd.ForEach(item =>
            {
                e.State = item.Item2;
                item.Item1.Invoke(e);
            });

            return e;
        }

        internal static LoadUrlBeginEventArgs GetByJob(IntPtr job)
        {
            if (_args.ContainsKey(job.ToInt64()) == false)
                return null;
            LoadUrlBeginEventArgs e;
            return _args.TryRemove(job.ToInt64(), out e) ? e : null;
        }
    }

    public class LoadUrlEndArgs : MiniblinkEventArgs
    {
        public wkeRequestType RequestMethod { get; internal set; }
        public string Url { get; internal set; }
        public IntPtr Job { get; internal set; }
        public byte[] Data { get; internal set; }
        public object State { get; internal set; }
        internal bool Modify;

        internal LoadUrlEndArgs()
        {
        }

        public void ReplaceData(byte[] data)
        {
            Modify = true;
            Data = data;
        }

        public string GetHeader(string name)
        {
            return MBApi.wkeNetGetHTTPHeaderFieldFromResponse(Job, name).ToUTF8String();
        }
    }

    public class WndMsgEventArgs : MiniblinkEventArgs
    {
        public IntPtr Handle { get; internal set; }
        public int Message { get; internal set; }
        public IntPtr WParam { get; internal set; }
        public IntPtr LParam { get; internal set; }
        public IntPtr? Result { get; set; }

        internal WndMsgEventArgs()
        {
        }
    }

    public class PaintUpdatedEventArgs : MiniblinkEventArgs
    {
        public IntPtr WebView { get; internal set; }
        public IntPtr Param { get; internal set; }
        public IntPtr Hdc { get; internal set; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public bool Cancel { get; set; }

        internal PaintUpdatedEventArgs()
        {
        }
    }

    public class DownloadEventArgs : MiniblinkEventArgs
    {
        public string Url { get; internal set; }
    }

    public class AlertEventArgs : EventArgs
    {
        public object Window { get; set; }

        internal AlertEventArgs()
        {

        }
    }

    public class ConfirmEventArgs : MiniblinkEventArgs
    {
        public object Window { get; set; }
        public bool? Result { get; set; }

        internal ConfirmEventArgs()
        {

        }
    }

    public class PromptEventArgs : MiniblinkEventArgs
    {
        public object Window { get; set; }
        public string Result { get; set; }

        internal PromptEventArgs()
        {

        }
    }

    public class DidCreateScriptContextEventArgs : MiniblinkEventArgs
    {
        public FrameContext Frame { get; internal set; }

        internal DidCreateScriptContextEventArgs()
        {

        }
    }

    public class WindowOpenEventArgs : MiniblinkEventArgs
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public IDictionary<string, string> Specs { get; private set; }
        public bool Replace { get; set; }
        public string ReturnValue { get; set; }
        public bool LoadUrl { get; set; }

        internal WindowOpenEventArgs()
        {
            Specs = new Dictionary<string, string>();
            LoadUrl = true;
        }
    }
}