using Miniblink;
using MiniBlink;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfMiniBlink.Ime;

namespace WpfMiniBlink
{
    public class MiniblinkBrowser : Border, IMiniblink, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region 属性

        public IWpfKeyboardHandler WpfKeyboardHandler { get; set; }

        MBApi.TitleChangedCallback titleChangeCallback;
        MBApi.TitleChangedCallback titleChangeCallback2;
        wkeCreateViewCallback _wkeCreateViewCallback;
        Image ViewImage;
        WriteableBitmap _WriteBitMap;
        /// <summary>
        /// 绘图对象
        /// </summary>
        public WriteableBitmap WriteBitMap
        {
            get { return _WriteBitMap; }
            set { _WriteBitMap = value; OnPropertyChanged("WriteBitMap"); }
        }


        private int width, heigth;

        private bool DesignMode
        {
            get {
                return true;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr MiniblinkHandle { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DeviceParameter DeviceParameter { get; }

        private bool _fireDropFile;

        public bool FireDropFile
        {
            get { return _fireDropFile; }
            set
            {
                if (_fireDropFile == value)
                {
                    return;
                }

                if (value)
                {
                    //DragOver += DragFileDrop;
                    //DragEnter += DragFileEnter;
                }
                else
                {
                    //DragOver -= DragFileDrop;
                    //DragEnter -= DragFileEnter;
                }

                _fireDropFile = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Url => MBApi.wkeGetURL(MiniblinkHandle).ToUTF8String() ?? string.Empty;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDocumentReady => MBApi.wkeIsDocumentReady(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DocumentTitle => MBApi.wkeGetTitle(MiniblinkHandle).ToUTF8String() ?? string.Empty;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DocumentWidth => MBApi.wkeGetWidth(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DocumentHeight => MBApi.wkeGetHeight(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ContentWidth => MBApi.wkeGetContentWidth(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ContentHeight => MBApi.wkeGetContentHeight(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ViewWidth => MBApi.wkeGetWidth(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ViewHeight => MBApi.wkeGetHeight(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanGoBack => MBApi.wkeCanGoBack(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanGoForward => MBApi.wkeCanGoForward(MiniblinkHandle);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Zoom
        {
            get { return DesignMode ? 0 : MBApi.wkeGetZoomFactor(MiniblinkHandle); }
            set
            {
                if (!DesignMode)
                {
                    MBApi.wkeSetZoomFactor(MiniblinkHandle, value);
                }
            }
        }

        private bool _cookieEnabled = true;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CookieEnabled
        {
            get { return _cookieEnabled; }
            set
            {
                _cookieEnabled = value;

                if (!DesignMode)
                {
                    MBApi.wkeSetCookieEnabled(MiniblinkHandle, _cookieEnabled);

                    if (_cookieEnabled)
                    {
                        LoadUrlBegin -= ClearCookie;
                    }
                    else
                    {
                        LoadUrlBegin += ClearCookie;
                    }
                }
            }
        }

        private void ClearCookie(object sender, LoadUrlBeginEventArgs e)
        {
            if (_cookieEnabled) return;
            MBApi.wkePerformCookieCommand(MiniblinkHandle, wkeCookieCommand.ClearAllCookies);
            MBApi.wkePerformCookieCommand(MiniblinkHandle, wkeCookieCommand.ClearSessionCookies);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string UserAgent
        {
            get { return DesignMode ? "" : MBApi.wkeGetUserAgent(MiniblinkHandle).ToUTF8String(); }
            set
            {
                if (!DesignMode)
                {
                    MBApi.wkeSetUserAgent(MiniblinkHandle, value);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollTop
        {
            get
            {
                return DesignMode
                    ? 0
                    : Convert.ToInt32(
                        RunJs("return Math.max(document.documentElement.scrollTop,document.body.scrollTop)"));
            }
            set
            {
                if (!DesignMode)
                {
                    ScrollTo(ScrollLeft, value);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollLeft
        {
            get
            {
                return DesignMode
                    ? 0
                    : Convert.ToInt32(
                        RunJs("return Math.max(document.documentElement.scrollLeft,document.body.scrollLeft)"));
            }
            set
            {
                if (!DesignMode)
                {
                    ScrollTo(value, ScrollTop);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollHeight
        {
            get
            {
                return DesignMode
                    ? 0
                    : Convert.ToInt32(
                        RunJs("return Math.max(document.documentElement.scrollHeight,document.body.scrollHeight)"));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollWidth
        {
            get
            {
                return DesignMode
                    ? 0
                    : Convert.ToInt32(
                        RunJs("return Math.max(document.documentElement.scrollWidth,document.body.scrollWidth)"));
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 标题改变
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="param"></param>
        /// <param name="title"></param>
        void OnTitleChangedCallback(IntPtr webView, IntPtr param, IntPtr title)
        {

            if (OnTitleChange != null)
            {
                OnTitleChange(title.WKEToUTF8String());
            }
        }

        /// <summary>
        /// 设置鼠标指针
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="param"></param>
        /// <param name="title"></param>
        void OnTitleChangedCallback2(IntPtr webView, IntPtr param, IntPtr title)
        {
            SetCursors();
        }

        private wkeDidCreateScriptContextCallback _wkeDidCreateScriptContextCallback;
        private EventHandler<DidCreateScriptContextEventArgs> _didCreateScriptContextCallback;

        public event EventHandler<DidCreateScriptContextEventArgs> DidCreateScriptContext
        {
            add
            {
                if (_wkeDidCreateScriptContextCallback == null)
                {
                    _wkeDidCreateScriptContextCallback = new wkeDidCreateScriptContextCallback(onWkeDidCreateScriptContextCallback);
                    MBApi.wkeOnDidCreateScriptContext(MiniblinkHandle, _wkeDidCreateScriptContextCallback, IntPtr.Zero);
                }

                _didCreateScriptContextCallback += value;
            }
            remove { _didCreateScriptContextCallback -= value; }
        }

        public event EventHandler<WindowOpenEventArgs> WindowOpen;

        /// <summary>
        /// 新窗口打开
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="specs"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        protected virtual WindowOpenEventArgs OnWindowOpen(string url, string name,
            IDictionary<string, string> specs, bool replace)
        {
            var args = new WindowOpenEventArgs
            {
                Name = name,
                Url = url,
                Replace = replace
            };
            if (specs != null)
            {
                foreach (var item in specs)
                {
                    args.Specs.Add(item);
                }
            }

            WindowOpen?.Invoke(this, args);

            if (args.LoadUrl && url != null)
            {
                LoadUri(url);
            }

            return args;
        }

        protected virtual void onWkeDidCreateScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frame,
            IntPtr context,
            int extensionGroup, int worldId)
        {
            var e = new DidCreateScriptContextEventArgs
            {
                Frame = new FrameContext(this, frame)
            };
            _didCreateScriptContextCallback?.Invoke(this, e);
        }

        private wkeURLChangedCallback2 _wkeUrlChanged;
        private EventHandler<UrlChangedEventArgs> _urlChanged;

        public event EventHandler<UrlChangedEventArgs> UrlChanged
        {
            add
            {
                if (_wkeUrlChanged == null)
                {
                    MBApi.wkeOnURLChanged2(MiniblinkHandle, _wkeUrlChanged = new wkeURLChangedCallback2(OnUrlChanged),
                        IntPtr.Zero);
                }

                _urlChanged += value;
            }
            remove { _urlChanged -= value; }
        }

        /// <summary>
        /// 地址改变
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="param"></param>
        /// <param name="frame"></param>
        /// <param name="url"></param>
        protected virtual void OnUrlChanged(IntPtr mb, IntPtr param, IntPtr frame, IntPtr url)
        {
            _urlChanged?.Invoke(this, new UrlChangedEventArgs
            {
                Url = url.WKEToUTF8String(),
                Frame = new FrameContext(this, frame)
            });
        }

        private wkeNavigationCallback _wkeNavigateBefore;
        private EventHandler<NavigateEventArgs> _navigateBefore;
        /// <summary>
        /// 跳转之前的处理
        /// </summary>
        public event EventHandler<NavigateEventArgs> NavigateBefore
        {
            add
            {
                if (_wkeNavigateBefore == null)
                {
                    MBApi.wkeOnNavigation(MiniblinkHandle,
                        _wkeNavigateBefore = new wkeNavigationCallback(OnNavigateBefore),
                        IntPtr.Zero);
                }

                _navigateBefore += value;
            }
            remove
            {
                _navigateBefore -= value;

                if (_navigateBefore == null)
                {
                    MBApi.wkeOnNavigation(MiniblinkHandle, null, IntPtr.Zero);
                }
            }
        }

        protected virtual byte OnNavigateBefore(IntPtr mb, IntPtr param, wkeNavigationType type, IntPtr url)
        {
            if (_navigateBefore == null)
                return 1;

            var e = new NavigateEventArgs
            {
                Url = url.WKEToUTF8String()
            };
            switch (type)
            {
                case wkeNavigationType.BackForward:
                    e.Type = NavigateType.BackForward;
                    break;
                case wkeNavigationType.FormReSubmit:
                    e.Type = NavigateType.ReSubmit;
                    break;
                case wkeNavigationType.FormSubmit:
                    e.Type = NavigateType.Submit;
                    break;
                case wkeNavigationType.LinkClick:
                    e.Type = NavigateType.LinkClick;
                    break;
                case wkeNavigationType.ReLoad:
                    e.Type = NavigateType.ReLoad;
                    break;
                case wkeNavigationType.Other:
                    e.Type = NavigateType.Other;
                    break;
                default:
                    throw new Exception("未知的重定向类型：" + type);
            }
            OnNavigateBefore(e);

            return (byte)(e.Cancel ? 0 : 1);
        }

        protected virtual void OnNavigateBefore(NavigateEventArgs args)
        {
            if(_navigateBefore!=null)
                _navigateBefore(this, args);
        }

        private wkeDocumentReady2Callback _wkeDocumentReady;
        private EventHandler<DocumentReadyEventArgs> _documentReady;

        public event EventHandler<DocumentReadyEventArgs> DocumentReady
        {
            add
            {
                if (_wkeDocumentReady == null)
                {
                    MBApi.wkeOnDocumentReady2(MiniblinkHandle,
                        _wkeDocumentReady = new wkeDocumentReady2Callback(OnDocumentReady),
                        IntPtr.Zero);
                }

                _documentReady += value;
            }
            remove { _documentReady -= value; }
        }

        /// <summary>
        /// 页面加载完成
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="param"></param>
        /// <param name="frameId"></param>
        protected virtual void OnDocumentReady(IntPtr mb, IntPtr param, IntPtr frameId)
        {
            _documentReady?.Invoke(this, new DocumentReadyEventArgs
            {
                Frame = new FrameContext(this, frameId)
            });
        }

        private wkeConsoleCallback _wkeConsoleMessage;
        private EventHandler<ConsoleMessageEventArgs> _consoleMessage;

        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage
        {
            add
            {
                if (_wkeConsoleMessage == null)
                {
                    MBApi.wkeOnConsole(MiniblinkHandle, _wkeConsoleMessage = new wkeConsoleCallback(OnConsoleMessage),
                        IntPtr.Zero);
                }

                _consoleMessage += value;
            }
            remove
            {
                _consoleMessage -= value;

                if (_consoleMessage == null)
                {
                    MBApi.wkeOnConsole(MiniblinkHandle, null, IntPtr.Zero);
                }
            }
        }

        protected virtual void OnConsoleMessage(IntPtr mb, IntPtr param, wkeConsoleLevel level, IntPtr message,
            IntPtr sourceName, uint sourceLine, IntPtr stackTrace)
        {
            _consoleMessage?.Invoke(this, new ConsoleMessageEventArgs
            {
                Level = level,
                Message = message.WKEToUTF8String(),
                SourceLine = (int)sourceLine,
                SourceName = sourceName.WKEToUTF8String(),
                StackTrace = stackTrace.WKEToUTF8String()
            });
        }

        private wkeNetResponseCallback _wkeNetResponse;
        private EventHandler<NetResponseEventArgs> _netResponse;

        public event EventHandler<NetResponseEventArgs> NetResponse
        {
            add
            {
                if (_wkeNetResponse == null)
                {
                    MBApi.wkeNetOnResponse(MiniblinkHandle, _wkeNetResponse = new wkeNetResponseCallback(OnNetResponse),
                        IntPtr.Zero);
                }

                _netResponse += value;
            }
            remove
            {
                _netResponse -= value;

                if (_netResponse == null)
                {
                    MBApi.wkeNetOnResponse(MiniblinkHandle, null, IntPtr.Zero);
                }
            }
        }

        protected virtual bool OnNetResponse(IntPtr mb, IntPtr param, string url, IntPtr job)
        {
            if (_netResponse == null)
                return true;

            var e = new NetResponseEventArgs
            {
                Job = job,
                Url = url,
                ContentType = MBApi.wkeNetGetMIMEType(job).ToUTF8String()
            };
            _netResponse(this, e);

            if (e.Data != null)
            {
                NetSetData(e.Job, e.Data);
                return true;
            }

            return e.Cancel;
        }

        private wkeLoadUrlBeginCallback _wkeLoadUrlBegin;
        private wkeLoadUrlEndCallback _wkeLoadUrlEnd;

        public Action<string> OnTitleChange;

        private EventHandler<LoadUrlBeginEventArgs> _loadUrlBegin;

        public event EventHandler<LoadUrlBeginEventArgs> LoadUrlBegin
        {
            add
            {
                if (_wkeLoadUrlBegin == null)
                {
                    MBApi.wkeOnLoadUrlBegin(MiniblinkHandle,
                        _wkeLoadUrlBegin = new wkeLoadUrlBeginCallback(OnLoadUrlBegin),
                        IntPtr.Zero);
                    MBApi.wkeOnLoadUrlEnd(MiniblinkHandle, _wkeLoadUrlEnd = new wkeLoadUrlEndCallback(OnLoadUrlEnd),
                        IntPtr.Zero);
                }

                _loadUrlBegin += value;
            }
            remove { _loadUrlBegin -= value; }
        }

        private wkeDownloadCallback _wkeDownload;
        private EventHandler<DownloadEventArgs> _download;

        public event EventHandler<DownloadEventArgs> Download
        {
            add
            {
                if (_wkeDownload == null)
                {
                    MBApi.wkeOnDownload(MiniblinkHandle, _wkeDownload = new wkeDownloadCallback(OnDownload),
                        IntPtr.Zero);
                }

                _download += value;
            }
            remove { _download -= value; }
        }

        public event EventHandler<AlertEventArgs> AlertBefore;

        protected virtual void OnAlertBefore(AlertEventArgs e)
        {
            AlertBefore?.Invoke(this, e);
        }

        public event EventHandler<ConfirmEventArgs> ConfirmBefore;

        protected virtual void OnConfirmBefore(ConfirmEventArgs e)
        {
            ConfirmBefore?.Invoke(this, e);
        }

        public event EventHandler<PromptEventArgs> PromptBefore;

        protected virtual void OnPromptBefore(PromptEventArgs e)
        {
            PromptBefore?.Invoke(this, e);
        }

        protected virtual byte OnDownload(IntPtr mb, IntPtr param, IntPtr url)
        {
            var e = new DownloadEventArgs
            {
                Url = url.ToUTF8String()
            };
            _download?.Invoke(this, e);
            return 0;
        }

        public event EventHandler<PaintUpdatedEventArgs> PaintUpdated;

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="param"></param>
        /// <param name="hdc"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        protected virtual void OnPaintUpdated(IntPtr mb, IntPtr param, IntPtr hdc, int x, int y, int w, int h)
        {
            var e = new PaintUpdatedEventArgs
            {
                WebView = mb,
                Param = param,
                Hdc = hdc,
                X = x,
                Y = y,
                Width = w,
                Height = h
            };
            PaintUpdated?.Invoke(this, e);

            if (!e.Cancel)
            {
                _browserPaintUpdated(this, e);
            }
        }

        private void JobCompleted(NetJob job)
        {
            if (job.Data != null)
            {
                this.UIInvoke(() =>
                {
                    if (job.ResponseContentType != null)
                    {
                        MBApi.wkeNetSetMIMEType(job.Handle, job.ResponseContentType);
                    }

                    NetSetData(job.Handle, job.Data);
                    MBApi.wkeNetContinueJob(job.Handle);
                });
            }
            else
            {
                this.UIInvoke(() =>
                {
                    if (job.BeginArgs.HookRequest)
                    {
                        if (job.BeginArgs.IsLocalFile)
                        {
                            OnLoadUrlEnd(job.WebView, job.Handle);
                        }
                        else
                        {
                            MBApi.wkeNetHookRequest(job.Handle);
                        }
                    }

                    MBApi.wkeNetContinueJob(job.Handle);
                });
            }
        }

        protected virtual bool OnLoadUrlBegin(IntPtr mb, IntPtr param, IntPtr url, IntPtr job)
        {
            if (_loadUrlBegin == null)
                return false;
            var rawurl = url.ToUTF8String();
            var e = new LoadUrlBeginEventArgs
            {
                Job = new NetJob(mb, job, JobCompleted) { Url = rawurl },
                Url = rawurl,
                RequestMethod = MBApi.wkeNetGetRequestMethod(job)
            };
            e.Job.BeginArgs = e;
            _loadUrlBegin(this, e);

            if (e.Job.IsAsync)
            {
                return false;
            }

            if (e.Data != null)
            {
                if (!e.HookRequest || !OnLoadUrlEnd(mb, job, e.Data))
                {
                    NetSetData(job, e.Data);
                }
                MBApi.wkeNetCancelRequest(job);
                return true;
            }

            if (e.HookRequest)
            {
                if (e.IsLocalFile)
                {
                    OnLoadUrlEnd(mb, job, e.Data);
                    MBApi.wkeNetCancelRequest(job);
                    return true;
                }

                MBApi.wkeNetHookRequest(job);
                return false;
            }

            if (e.Cancel)
            {
                NetSetData(job);
            }

            return e.Cancel;
        }

        private void OnLoadUrlEnd(IntPtr mb, IntPtr param, IntPtr url, IntPtr job, IntPtr buf, int length)
        {
            var data = new byte[length];
            if (buf != IntPtr.Zero)
                Marshal.Copy(buf, data, 0, length);
            OnLoadUrlEnd(mb, job, data);
        }

        protected virtual bool OnLoadUrlEnd(IntPtr mb, IntPtr job, byte[] data = null)
        {
            var begin = LoadUrlBeginEventArgs.GetByJob(job);
            if (begin != null)
            {
                var end = begin.OnLoadUrlEnd(data);
                if (end.Modify || begin.IsLocalFile)
                {
                    NetSetData(job, end.Data);
                    return true;
                }
            }
            return false;
        }

        private static void NetSetData(IntPtr job, byte[] data = null)
        {
            if (data != null && data.Length > 0)
            {
                MBApi.wkeNetSetData(job, data, data.Length);
            }
            else
            {
                MBApi.wkeNetSetData(job, new byte[] { 0 }, 1);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 打开调试工具
        /// </summary>
        public void ShowDevTools()
        {
            var path = Path.Combine(System.Environment.CurrentDirectory, "front_end", "inspector.html");
            MBApi.wkeShowDevtools(MiniblinkHandle, path, null, IntPtr.Zero);
        }

        /// <summary>
        /// 执行js代码
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public object RunJs(string script)
        {
            var es = MBApi.wkeGlobalExec(MiniblinkHandle);
            return MBApi.jsEvalExW(es, script, true).ToValue(es);
        }

        /// <summary>
        /// 调用js方法
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object CallJsFunc(string funcName, params object[] param)
        {
            var es = MBApi.wkeGlobalExec(MiniblinkHandle);
            var func = MBApi.jsGetGlobal(es, funcName);
            if (func == 0)
                throw new WKEFunctionNotFondException(funcName);
            var args = param.Select(i => i.ToJsValue(es)).ToArray();
            return MBApi.jsCall(es, func, MBApi.jsUndefined(), args, args.Length).ToValue(es);
        }

        public void BindNetFunc(NetFunc func)
        {
            var funcvalue = new wkeJsNativeFunction((es, state) =>
            {
                var handle = GCHandle.FromIntPtr(state);
                var nfunc = (NetFunc)handle.Target;
                var arglen = MBApi.jsArgCount(es);
                var args = new List<object>();
                for (var i = 0; i < arglen; i++)
                {
                    args.Add(MBApi.jsArg(es, i).ToValue(es));
                }

                return nfunc.OnFunc(args.ToArray()).ToJsValue(es);
            });
            _ref[func.Name] = func;

            var ptr = GCHandle.ToIntPtr(GCHandle.Alloc(func));

            MBApi.wkeJsBindFunction(func.Name, funcvalue, ptr, 0);
        }

        public void SetHeadlessEnabled(bool enable)
        {
            MBApi.wkeSetHeadlessEnabled(MiniblinkHandle, enable);
        }

        public void SetNpapiPluginsEnable(bool enable)
        {
            MBApi.wkeSetNpapiPluginsEnabled(MiniblinkHandle, enable);
        }

        /// <summary>
        /// 设置跨域限制
        /// </summary>
        /// <param name="enable"></param>
        public void SetCspCheckEnable(bool enable)
        {
            MBApi.wkeSetCspCheckEnable(MiniblinkHandle, enable);
        }

        public void SetTouchEnabled(bool enable)
        {
            MBApi.wkeSetTouchEnabled(MiniblinkHandle, enable);
        }

        public void SetMouseEnabled(bool enable)
        {
            MBApi.wkeSetMouseEnabled(MiniblinkHandle, enable);
        }

        public bool GoForward()
        {
            return MBApi.wkeGoForward(MiniblinkHandle);
        }

        public void EditorSelectAll()
        {
            MBApi.wkeEditorSelectAll(MiniblinkHandle);
        }

        public void EditorUnSelect()
        {
            MBApi.wkeEditorUnSelect(MiniblinkHandle);
        }

        public void EditorCopy()
        {
            MBApi.wkeEditorCopy(MiniblinkHandle);
        }

        public void EditorCut()
        {
            MBApi.wkeEditorCut(MiniblinkHandle);
        }

        public void EditorPaste()
        {
            MBApi.wkeEditorPaste(MiniblinkHandle);
        }

        public void EditorDelete()
        {
            MBApi.wkeEditorDelete(MiniblinkHandle);
        }

        public void EditorUndo()
        {
            MBApi.wkeEditorUndo(MiniblinkHandle);
        }

        public void EditorRedo()
        {
            MBApi.wkeEditorRedo(MiniblinkHandle);
        }

        public bool GoBack()
        {
            return MBApi.wkeGoBack(MiniblinkHandle);
        }

        public void SetProxy(WKEProxy proxy)
        {
            MBApi.wkeSetViewProxy(MiniblinkHandle, proxy);
        }

        public void LoadUri(string uri)
        {
            if (string.IsNullOrEmpty(uri?.Trim()))
                return;

            if (uri.SW("http:") || uri.SW("https:"))
            {
                MBApi.wkeLoadURL(MiniblinkHandle, uri);
            }
            else
            {
                MBApi.wkeLoadFileW(MiniblinkHandle, uri);
            }
        }

        public void LoadHtml(string html, string baseUrl = null)
        {
            if (baseUrl == null)
            {
                MBApi.wkeLoadHTML(MiniblinkHandle, html);
            }
            else
            {
                MBApi.wkeLoadHtmlWithBaseUrl(MiniblinkHandle, html, baseUrl);
            }
        }

        public void StopLoading()
        {
            MBApi.wkeStopLoading(MiniblinkHandle);
        }

        public void Reload()
        {
            MBApi.wkeReload(MiniblinkHandle);
        }

        #endregion

        internal static MiniblinkBrowser InvokeBro { get; private set; }
        private static string _popHookName = "func" + Guid.NewGuid().ToString().Replace("-", "");
        private static string _openHookName = "func" + Guid.NewGuid().ToString().Replace("-", "");

        private static bool IsSetDefaultStyleKey = false;
        private static object DefaultStyleKeyLock = new object();

        private EventHandler<PaintUpdatedEventArgs> _browserPaintUpdated;
        private Hashtable _ref = new Hashtable();
        public IList<ILoadResource> LoadResourceHandlerList { get; private set; }
        public CookieCollection Cookies => GetCookies();

        public MiniblinkBrowser()
        {

            //强制允许控件获取焦点
            Focusable = true;
            //去除选中焦点样式
            FocusVisualStyle = null;

            lock (DefaultStyleKeyLock)
            {
                if (!IsSetDefaultStyleKey)
                {
                    IsSetDefaultStyleKey = true;
                    DefaultStyleKeyProperty.OverrideMetadata(
                      typeof(MiniblinkBrowser),
                      new System.Windows.FrameworkPropertyMetadata(typeof(MiniblinkBrowser)));
                }
            }
            ViewImage = new System.Windows.Controls.Image();

            ViewImage.Stretch = Stretch.None;

            this.Child = ViewImage;
            this.ViewImage.SetBinding(System.Windows.Controls.Image.SourceProperty, "WriteBitMap");

            this.DataContext = this;


            InvokeBro = InvokeBro ?? this;
            LoadResourceHandlerList = new List<ILoadResource>();

            if (!Utils.IsDesignMode())
            {
                if (MBApi.wkeIsInitialize() == false)
                {
                    MBApi.wkeInitialize();
                }

                MiniblinkHandle = MBApi.wkeCreateWebView();

                if (MiniblinkHandle == IntPtr.Zero)
                {
                    throw new WKECreateException();
                }
                MBApi.wkeSetDragEnable(MiniblinkHandle, false);
                MBApi.wkeSetDragDropEnable(MiniblinkHandle, false);
                //MBApi.wkeSetHandle(MiniblinkHandle, Handle);
                MBApi.wkeSetNavigationToNewWindowEnable(MiniblinkHandle, true);

                titleChangeCallback = OnTitleChangedCallback;
                MBApi.wkeOnTitleChanged(MiniblinkHandle, titleChangeCallback, IntPtr.Zero);

                titleChangeCallback2 = OnTitleChangedCallback2;

                //设置鼠标
                MBApi.wkeOnMouseOverUrlChanged(MiniblinkHandle, titleChangeCallback2, IntPtr.Zero);

                _wkeCreateViewCallback = OnCreateView;

                MBApi.wkeOnCreateView(MiniblinkHandle, _wkeCreateViewCallback, IntPtr.Zero);
                
                _browserPaintUpdated += BrowserPaintUpdated;

                var wkePaintUpdated = new wkePaintUpdatedCallback(OnPaintUpdated);
                _ref.Add(Guid.NewGuid(), wkePaintUpdated);

                MBApi.wkeOnPaintBitUpdated(MiniblinkHandle, wkePaintUpdated);

                LoadUrlBegin += LoadResource;
                DidCreateScriptContext += HookPop;
                DeviceParameter = new DeviceParameter(this);
                RegisterJsFunc();
                WpfKeyboardHandler = new WpfImeKeyboardHandler(this);

                System.Windows.PresentationSource.AddSourceChangedHandler(this, PresentationSourceChangedHandler);

                //销毁
                Dispatcher.ShutdownStarted += (object sender, EventArgs e) =>
                {
                    DestroyCallback();
                    LoadResourceHandlerList.Clear();
                    _ref.Clear();
                };

            }
        }


        HwndSource source;
        System.Windows.Window sourceWindow;
        private void PresentationSourceChangedHandler(object sender, System.Windows.SourceChangedEventArgs args)
        {
            if (args.NewSource != null)
            {
                source = (HwndSource)args.NewSource;

                WpfKeyboardHandler.Setup(source);

                var matrix = source.CompositionTarget.TransformToDevice;

                var window = source.RootVisual as System.Windows.Window;
                if (window != null)
                {
                    sourceWindow = window;
                }
            }
            else if (args.OldSource != null)
            {
                WpfKeyboardHandler.Dispose();

                var window = args.OldSource.RootVisual as System.Windows.Window;
                if (window != null)
                {
                    sourceWindow = null;
                }
            }
        }

        private IntPtr OnCreateView(IntPtr mb, IntPtr param, wkeNavigationType type, IntPtr url, IntPtr windowFeatures)
        {
            if (type == wkeNavigationType.LinkClick)
            {
                var e = new NavigateEventArgs
                {
                    Url = url.WKEToUTF8String(),
                    Type = NavigateType.BlankLink
                };
                OnNavigateBefore(e);
            }
            else
            {
                OnNavigateBefore(mb, param, type, url);
            }

            return IntPtr.Zero;
        }

        private void DestroyCallback()
        {
            MBApi.wkeOnPaintBitUpdated(MiniblinkHandle, null);
            MBApi.wkeOnURLChanged2(MiniblinkHandle, null, IntPtr.Zero);
            MBApi.wkeOnNavigation(MiniblinkHandle, null, IntPtr.Zero);
            MBApi.wkeOnDocumentReady2(MiniblinkHandle, null, IntPtr.Zero);
            MBApi.wkeOnConsole(MiniblinkHandle, null, IntPtr.Zero);
            MBApi.wkeNetOnResponse(MiniblinkHandle, null, IntPtr.Zero);
            MBApi.wkeOnLoadUrlBegin(MiniblinkHandle, null, IntPtr.Zero);
            MBApi.wkeOnLoadUrlEnd(MiniblinkHandle, null, IntPtr.Zero);
            MBApi.wkeOnDownload(MiniblinkHandle, null, IntPtr.Zero);
            MBApi.wkeOnCreateView(MiniblinkHandle, null, IntPtr.Zero);
        }


        System.Drawing.Size oldSize;
        private void BrowserPaintUpdated(object sender, PaintUpdatedEventArgs e)
        {
            if (width > 0 && heigth > 0)
            {
                width = (int)ActualWidth;
                heigth = (int)ActualHeight;

                if (oldSize.Width == (int)width && oldSize.Height == (int)heigth && WriteBitMap != null)
                {
                    WriteBitMap.WritePixels(new System.Windows.Int32Rect(0, 0, WriteBitMap.PixelWidth, WriteBitMap.PixelHeight), e.Hdc, width * heigth * 4, width * 4);
                }
                else
                {
                    try
                    {
                        BitmapPalette palette = new BitmapPalette(new List<System.Windows.Media.Color>() { Colors.Blue, Colors.Green, Colors.Red });
                        BitmapSource src = BitmapSource.Create((int)width, (int)heigth, 96, 96, PixelFormats.Bgra32, palette, e.Hdc, (int)width * (int)heigth * 4, (int)width * 4);

                        WriteBitMap = new WriteableBitmap(src);
                    }
                    catch (System.AccessViolationException ex)
                    {

                    }

                    oldSize = new System.Drawing.Size((int)width, (int)heigth);
                }

            }

        }

        public void ScrollTo(int x, int y)
        {
            if (IsDocumentReady)
            {
                RunJs($"window.scrollTo({x},{y})");
            }
        }

        /// <summary>
        /// 注册后台方法
        /// </summary>
        /// <param name="target"></param>
        public void RegisterNetFunc(object target)
        {
            var tg = target;
            var methods = tg.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<NetFuncAttribute>();
                if (attr == null) continue;
                BindNetFunc(new NetFunc(attr.Name ?? method.Name, ctx =>
                {
                    var m = (MethodInfo)ctx.State;
                    object ret;
                    var mps = m.GetParameters();
                    if (mps.Length < 1)
                    {
                        ret = m.Invoke(tg, null);
                    }
                    else
                    {
                        var param = ctx.Paramters;
                        var mpvs = new object[mps.Length];
                        for (var i = 0; i < mps.Length; i++)
                        {
                            var mp = mps[i];
                            var v = param.Length > i ? param[i] : null;
                            if (v != null)
                            {
                                var pt = mp.ParameterType;
                                if (pt.IsGenericType)
                                {
                                    if (pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        pt = pt.GetGenericArguments().First();
                                    }
                                }


                                if (pt == typeof(DateTime) && !(v is DateTime))
                                {
                                    long l_date;
                                    if (long.TryParse(v.ToString(), out l_date))
                                    {
                                        v = l_date.ToDate();
                                    }
                                }

                                if (v is JsFunc || pt == typeof(object) || pt == typeof(ExpandoObject))
                                {
                                    mpvs[i] = v;
                                }
                                else
                                {
                                    mpvs[i] = Convert.ChangeType(v, pt);
                                }
                            }
                            else if (mp.ParameterType.IsValueType)
                            {
                                mpvs[i] = Activator.CreateInstance(mp.ParameterType);
                            }
                            else
                            {
                                mpvs[i] = null;
                            }
                        }

                        ret = m.Invoke(tg, mpvs);
                    }

                    return ret;
                }, method));
            }
        }


        private void LoadResource(object sender, LoadUrlBeginEventArgs e)
        {
            if (LoadResourceHandlerList.Count < 1)
                return;
            if (e.RequestMethod != wkeRequestType.Get)
                return;
            var url = e.Url;
            if (url.SW("http:") == false && url.SW("https:") == false)
                return;

            var uri = new Uri(url);

            foreach (var handler in LoadResourceHandlerList.ToArray())
            {
                if (handler.Domain.Equals(uri.Host, StringComparison.OrdinalIgnoreCase) == false)
                    continue;
                e.IsLocalFile = true;
                var data = handler.ByUri(uri);
                if (data != null)
                {
                    e.Data = data;
                    break;
                }
            }
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="callback"></param>
        public void DrawToBitmap(Action<ScreenshotImage> callback)
        {
            new DrawToBitmapUtil(this).ToImage(callback);
        }

        /// <summary>
        /// 绑定窗口弹窗事件
        /// </summary>
        private void RegisterJsFunc()
        {
            //BindNetFunc(new NetFunc(_popHookName, OnHookPop));
            //BindNetFunc(new NetFunc(_openHookName, OnHookWindowOpen));
        }

        private void HookPop(object sender, DidCreateScriptContextEventArgs e)
        {
            var js = string.Join(".", GetType().Namespace, "Files", "hook.js");

            using (var sm = GetType().Assembly.GetManifestResourceStream(js))
            {
                if (sm != null)
                {
                    using (var reader = new StreamReader(sm, Encoding.UTF8))
                    {
                        js = reader.ReadToEnd();
                    }
                }
            }

            js = $@"var popHookName='{_popHookName}';
                    var openHookName='{_openHookName}';"
                 + js;

            e.Frame.RunJs(js);
        }


        /// <summary>
        /// Hook消息弹出框，暂未实现
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private object OnHookPop(NetFuncContext context)
        {
            if (context.Paramters.Length < 1)
            {
                return null;
            }
            var type = context.Paramters[0].ToString().ToLower();

            if ("alert" == type)
            {
                var msg = "";
                var title = new Uri(Url).Host;
                if (context.Paramters.Length > 1 && context.Paramters[1] != null)
                {
                    msg = context.Paramters[1].ToString();
                }

                if (context.Paramters.Length > 2 && context.Paramters[2] != null)
                {
                    dynamic opt = context.Paramters[2];
                    if (opt.title != null)
                    {
                        title = opt.title.ToString();
                    }
                }

                //OnAlert(msg, title);
                return null;
            }

            if ("confirm" == type)
            {
                var msg = "";
                var title = new Uri(Url).Host;
                if (context.Paramters.Length > 1 && context.Paramters[1] != null)
                {
                    msg = context.Paramters[1].ToString();
                }

                if (context.Paramters.Length > 2 && context.Paramters[2] != null)
                {
                    dynamic opt = context.Paramters[2];
                    if (opt.title != null)
                    {
                        title = opt.title.ToString();
                    }
                }

                //var e = OnConfirm(msg, title);
                return null;//e.Result.GetValueOrDefault();
            }

            if ("prompt" == type)
            {
                var msg = "";
                var input = "";
                var title = new Uri(Url).Host;
                if (context.Paramters.Length > 1 && context.Paramters[1] != null)
                {
                    msg = context.Paramters[1].ToString();
                }

                if (context.Paramters.Length > 2 && context.Paramters[2] != null)
                {
                    input = context.Paramters[2].ToString();
                }

                if (context.Paramters.Length > 3 && context.Paramters[3] != null)
                {
                    dynamic opt = context.Paramters[3];
                    if (opt.title != null)
                    {
                        title = opt.title.ToString();
                    }
                }

                //var e = OnPrompt(msg, input, title);
                return null;//e.Result;
            }

            return null;
        }

        private object OnHookWindowOpen(NetFuncContext context)
        {
            string url = null;
            string name = null;
            string specs = null;
            string replace = null;
            var map = new Dictionary<string, string>();

            if (context.Paramters.Length > 0 && context.Paramters[0] != null)
            {
                url = context.Paramters[0].ToString();
            }

            if (context.Paramters.Length > 1 && context.Paramters[1] != null)
            {
                name = context.Paramters[1].ToString();
            }

            if (context.Paramters.Length > 2 && context.Paramters[2] != null)
            {
                specs = context.Paramters[2].ToString();
            }

            if (context.Paramters.Length > 3 && context.Paramters[3] != null)
            {
                replace = context.Paramters[3].ToString();
            }

            if (specs != null)
            {
                var items = specs.Split(',');
                foreach (var item in items)
                {
                    var kv = item.Split('=');
                    if (kv.Length == 1)
                    {
                        map[kv[0]] = "";
                    }
                    else if (kv.Length == 2)
                    {
                        map[kv[0]] = kv[1];
                    }
                }
            }

            var navArgs = new NavigateEventArgs
            {
                Url = url,
                Type = NavigateType.WindowOpen
            };
            OnNavigateBefore(navArgs);
            if (navArgs.Cancel)
            {
                return null;
            }
            var e = OnWindowOpen(url, name, map, "true" == replace);
            return e.ReturnValue;
        }

        public void Print(Action<PrintDialog> callback)
        {
            new PrintUtil(this).Start(callback);
        }

        private CookieCollection GetCookies()
        {
            var file = "cookies.dat";

            if (File.Exists(file) == false)
            {
                return null;
            }

            MBApi.wkePerformCookieCommand(MiniblinkHandle, wkeCookieCommand.FlushCookiesToFile);
            var host = new Uri(Url).Host.ToLower();
            var cookies = new CookieCollection();
            var rows = File.ReadAllLines(file, Encoding.UTF8);

            foreach (var row in rows)
            {
                if (row.StartsWith("# ")) continue;
                var items = row.Split('\t');
                if (items.Length != 7) continue;
                var domain = items[0];
                var httpOnly = domain.StartsWith("#HttpOnly_");
                if (httpOnly)
                {
                    domain = domain.Substring(domain.IndexOf("_", StringComparison.Ordinal) + 1).ToLower();
                }

                if ("true".Equals(items[1], StringComparison.OrdinalIgnoreCase))
                {
                    if (host.EndsWith(domain) == false)
                    {
                        if (("." + host).Equals(domain) == false)
                        {
                            continue;
                        }
                    }
                }
                else if (host.Equals(domain) == false)
                {
                    continue;
                }

                var cookie = new Cookie
                {
                    HttpOnly = httpOnly,
                    Domain = domain.TrimStart('.'),
                    Path = items[2],
                    Secure = "true".Equals(items[3], StringComparison.OrdinalIgnoreCase),
                    Expires = new DateTime(1970, 1, 1).AddSeconds(long.Parse(items[4])),
                    Name = items[5],
                    Value = items[6]
                };
                cookies.Add(cookie);
            }

            return cookies;
        }



        #region 消息处理

        /// <summary>
        /// 重新绘制大小
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            width = (int)ActualWidth;
            heigth = (int)ActualHeight;
            if (!Utils.IsDesignMode() && MiniblinkHandle != IntPtr.Zero)
            {
                MBApi.wkeResize(MiniblinkHandle, (int)width, (int)heigth);
            }
        }


        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var key = e.SystemKey == Key.None ? e.Key : e.SystemKey;
            int code = KeyInterop.VirtualKeyFromKey(key);
            var flags = (uint)wkeKeyFlags.WKE_REPEAT;

            if (Utils.IsExtendedKey(key))
            {
                flags |= (uint)wkeKeyFlags.WKE_EXTENDED;
            }

            if (MBApi.wkeFireKeyDownEvent(MiniblinkHandle, code, 0, false))
            {
                e.Handled = false;
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            var key = e.SystemKey == Key.None ? e.Key : e.SystemKey;
            int code = KeyInterop.VirtualKeyFromKey(key);
            var flags = (uint)wkeKeyFlags.WKE_REPEAT;

            if (Utils.IsExtendedKey(key))
            {
                flags |= (uint)wkeKeyFlags.WKE_EXTENDED;
            }

            if (MBApi.wkeFireKeyUpEvent(MiniblinkHandle, code, 0, false))
            {
                e.Handled = false;
            }

            base.OnPreviewKeyUp(e);
        }


        protected override void OnGotFocus(System.Windows.RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (MiniblinkHandle != IntPtr.Zero)
            {
                MBApi.wkeSetFocus(MiniblinkHandle);
            }
        }

        protected override void OnLostFocus(System.Windows.RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (MiniblinkHandle != IntPtr.Zero)
            {
                MBApi.wkeKillFocus(MiniblinkHandle);
            }
        }

        private static uint GetMouseFlags(MouseEventArgs e)
        {
            uint flags = 0;


            if (e.LeftButton == MouseButtonState.Pressed)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_LBUTTON;
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_MBUTTON;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_RBUTTON;
            }

            

            //判断键盘按键
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_CONTROL;
            }
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_SHIFT;
            }
            return flags;
        }

        /// <summary>
        /// 文本输入
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            if (MiniblinkHandle != IntPtr.Zero)
            {
                for (int i = 0; i < e.Text.Length; i++)
                {
                    MBApi.wkeFireKeyPressEvent(MiniblinkHandle, e.Text[i], 0, false);

                }
            }
        }

        public new void TextInput(string text)
        {
            if (MiniblinkHandle != IntPtr.Zero)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    MBApi.wkeFireKeyPressEvent(MiniblinkHandle, text[i], 0, false);

                }
            }
        }


        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            var point = e.GetPosition(this);
            if (MiniblinkHandle != IntPtr.Zero)
            {
                uint flags = (uint)GetMouseFlags(e);
                MBApi.wkeFireMouseWheelEvent(MiniblinkHandle, (int)point.X, (int)point.Y, e.Delta, flags);
            }

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (MiniblinkHandle != IntPtr.Zero)
            {
                Focus();
                uint msg = 0;
                if (e.ChangedButton == MouseButton.Left)
                {
                    msg = (uint)WinConst.WM_LBUTTONDOWN;
                }
                else if (e.ChangedButton == MouseButton.Middle)
                {
                    msg = (uint)WinConst.WM_MBUTTONDOWN;
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    msg = (uint)WinConst.WM_RBUTTONDOWN;
                }
                var point = e.GetPosition(this);
                uint flags = GetMouseFlags(e);
                MBApi.wkeFireMouseEvent(MiniblinkHandle, msg, (int)point.X, (int)point.Y, flags);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (MiniblinkHandle != IntPtr.Zero)
            {
                uint msg = 0;
                if (e.ChangedButton == MouseButton.Left)
                {
                    msg = (uint)WinConst.WM_LBUTTONUP;
                }
                else if (e.ChangedButton == MouseButton.Middle)
                {
                    msg = (uint)WinConst.WM_MBUTTONUP;
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    msg = (uint)WinConst.WM_RBUTTONUP;
                }

                var point = e.GetPosition(this);

                uint flags = GetMouseFlags(e);

                MBApi.wkeFireMouseEvent(MiniblinkHandle, msg, (int)point.X, (int)point.Y, flags);
            }
        }



        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.MiniblinkHandle != IntPtr.Zero)
            {
                uint flags = GetMouseFlags(e);
                var point = e.GetPosition(this);
                MBApi.wkeFireMouseEvent(this.MiniblinkHandle, 0x200, (int)point.X, (int)point.Y, flags);
            }
        }


        void SetCursors()
        {
            switch (MBApi.wkeGetCursorInfoType(MiniblinkHandle))
            {
                case wkeCursorInfo.Pointer:
                    Cursor = null;
                    break;
                case wkeCursorInfo.Cross:
                    Cursor = Cursors.Cross;
                    break;
                case wkeCursorInfo.Hand:
                    Cursor = Cursors.Hand;
                    break;
                case wkeCursorInfo.IBeam:
                    Cursor = Cursors.IBeam;
                    break;
                case wkeCursorInfo.Wait:
                    Cursor = Cursors.Wait;
                    break;
                case wkeCursorInfo.Help:
                    Cursor = Cursors.Help;
                    break;
                case wkeCursorInfo.EastResize:
                    Cursor = Cursors.SizeWE;
                    break;
                case wkeCursorInfo.NorthResize:
                    Cursor = Cursors.SizeNS;
                    break;
                case wkeCursorInfo.NorthEastResize:
                    Cursor = Cursors.SizeNESW;
                    break;
                case wkeCursorInfo.NorthWestResize:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case wkeCursorInfo.SouthResize:
                    Cursor = Cursors.SizeWE;
                    break;
                case wkeCursorInfo.SouthEastResize:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case wkeCursorInfo.SouthWestResize:
                    Cursor = Cursors.SizeNESW;
                    break;
                case wkeCursorInfo.WestResize:
                    Cursor = Cursors.SizeWE;
                    break;
                case wkeCursorInfo.NorthSouthResize:
                    Cursor = Cursors.SizeNS;
                    break;
                case wkeCursorInfo.EastWestResize:
                    Cursor = Cursors.SizeWE;
                    break;
                case wkeCursorInfo.NorthEastSouthWestResize:
                    Cursor = Cursors.SizeAll;
                    break;
                case wkeCursorInfo.NorthWestSouthEastResize:
                    Cursor = Cursors.SizeAll;
                    break;
                case wkeCursorInfo.ColumnResize:
                    Cursor = null;
                    break;
                case wkeCursorInfo.RowResize:
                    Cursor = null;
                    break;
                default:
                    Cursor = null;
                    break;
            }
        }




        #endregion


        #region JSFunctin相互调用


        object _GlobalObjectJs = null;

        public object GlobalObjectJs
        {
            get { return _GlobalObjectJs; }
            set
            {

                _GlobalObjectJs = value;
                if (_GlobalObjectJs != this)
                {
                    BindJsFunc();
                }
            }
        }
        /// <summary>
        /// 绑定带有 JSFunctin 属性的方法到前台JS，实现前后台相互调用。
        /// </summary>
        public void BindJsFunc()
        {
            if (GlobalObjectJs == null)
            {
                GlobalObjectJs = this;
            }
            var att = GlobalObjectJs.GetType().GetMethods();
            //jsnaviteList.Clear();
            var result = new ArrayList();
            foreach (var item in att)
            {
                var xx = item.GetCustomAttributes(typeof(JSFunctin), true);
                if (xx != null && xx.Length != 0)
                {
                    var jsnav = new wkeJsNativeFunction((es, _param) =>
                    {
                        var xp = item.GetParameters();
                        var argcount = MBApi.jsArgCount(es);
                        long param = 0L;
                        if (xp != null && xp.Length != 0 && argcount != 0)
                        {

                            object[] listParam = new object[MBApi.jsArgCount(es)];
                            for (int i = 0; i < argcount; i++)
                            {
                                Type tType = xp[i].ParameterType;

                                var paramnow = MBApi.jsArg(es, i);
                                param = paramnow;
                                if (tType == typeof(int))
                                {
                                    listParam[i] = Convert.ChangeType(MBApi.jsToInt(es, paramnow), tType);
                                }
                                else
                                if (tType == typeof(double))
                                {
                                    listParam[i] = Convert.ChangeType(MBApi.jsToDouble(es, paramnow), tType);
                                }
                                else
                                if (tType == typeof(float))
                                {
                                    listParam[i] = Convert.ChangeType(MBApi.jsToFloat(es, paramnow), tType);
                                }
                                else
                                if (tType == typeof(bool))
                                {
                                    listParam[i] = Convert.ChangeType(MBApi.jsToBoolean(es, paramnow), tType);
                                }
                                else
                                {
                                    listParam[i] = Convert.ChangeType(MBApi.Utf8IntptrToString(MBApi.jsToString(es, paramnow)), tType);
                                }
                            }
                            try
                            {
                                var res = item.Invoke(GlobalObjectJs, listParam);
                                if (res != null)
                                {
                                    var mStr = Marshal.StringToHGlobalUni(res.ToString());
                                    return MBApi.jsStringW(es, mStr);//返回JS字符串
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                        else
                        {
                            var res = item.Invoke(GlobalObjectJs, null);
                            if (res != null)
                            {
                                var mStr = Marshal.StringToHGlobalUni(res.ToString());
                                return MBApi.jsStringW(es, mStr);//返回JS字符串
                            }
                        }
                        return param;
                    });

                    MBApi.wkeJsBindFunction(item.Name, jsnav, IntPtr.Zero, (uint)item.GetParameters().Length);
                    _ref[item.Name] = jsnav;
                }
            }
        } 
        #endregion
    }
}
