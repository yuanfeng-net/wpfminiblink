using System;
using System.Runtime.InteropServices;

namespace Miniblink
{
    internal class MBApi
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TitleChangedCallback(IntPtr webView, IntPtr param, IntPtr title);

        private const string DLL_x86 = "node_x86.dll";
        private const string DLL_x64 = "node_x64.dll";
        private static bool is64()
        {
            return IntPtr.Size == 8;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeIsInitialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsInitialize_x86();
        [DllImport(DLL_x64, EntryPoint = "wkeIsInitialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsInitialize_x64();

        public static bool wkeIsInitialize()
        {
            if (is64())
            {
                return wkeIsInitialize_x64() != 0;
            }

            return wkeIsInitialize_x86() != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeInitialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeInitialize_x86();
        [DllImport(DLL_x64, EntryPoint = "wkeInitialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeInitialize_x64();

        public static void wkeInitialize()
        {
            if (is64())
            {
                wkeInitialize_x64();
            }
            else
            {
                wkeInitialize_x86();
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeInitializeEx", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void wkeInitializeEx_x86(WKESettings settings);
        //[DllImport(DLL_x64, EntryPoint = "wkeInitializeEx", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void wkeInitializeEx_x64(WKESettings settings);

        //public static void wkeInitializeEx(WKESettings settings)
        //{
        //    if (is64())
        //    {
        //        wkeInitializeEx_x64(settings);
        //    }
        //    else
        //    {
        //        wkeInitializeEx_x86(settings);
        //    }
        //}

        //[DllImport(DLL_x86, EntryPoint = "wkeFinalize", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeFinalize();

        //[DllImport(DLL_x86, EntryPoint = "wkeConfigure", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeConfigure(WKESettings settings);

        //[DllImport(DLL_x86, EntryPoint = "wkeSetDebugConfig", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Unicode)]
        //public static extern void wkeSetDebugConfig(IntPtr webView, string debugString, string param);

        [DllImport(DLL_x86, EntryPoint = "wkeSetTouchEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetTouchEnabled_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport(DLL_x64, EntryPoint = "wkeSetTouchEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetTouchEnabled_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);

        public static void wkeSetTouchEnabled(IntPtr webView,  bool enable)
        {
            if (is64())
            {
                wkeSetTouchEnabled_x64(webView, enable);
            }
            else
            {
                wkeSetTouchEnabled_x86(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetMouseEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetMouseEnabled_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport(DLL_x64, EntryPoint = "wkeSetMouseEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetMouseEnabled_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);

        public static void wkeSetMouseEnabled(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetMouseEnabled_x86(webView, enable);
            }
            else
            {
                wkeSetMouseEnabled_x64(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetDeviceParameter", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void wkeSetDeviceParameter_x86(IntPtr webView, string device, string s, int i, float f);
        [DllImport(DLL_x64, EntryPoint = "wkeSetDeviceParameter", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void wkeSetDeviceParameter_x64(IntPtr webView, string device, string s, int i, float f);

        public static void wkeSetDeviceParameter(IntPtr webView, string type, string s, int i, float f)
        {
            if (is64())
            {
                wkeSetDeviceParameter_x64(webView, type, s, i, f);
            }
            else
            {
                wkeSetDeviceParameter_x86(webView, type, s, i, f);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeGetVersion", CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint wkeGetVersion();

        //[DllImport(DLL_x86, EntryPoint = "wkeGetVersionString", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr wkeGetVersionString();

        [DllImport(DLL_x86, EntryPoint = "wkeCreateWebView", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeCreateWebView_x86();
        [DllImport(DLL_x64, EntryPoint = "wkeCreateWebView", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeCreateWebView_x64();

        public static IntPtr wkeCreateWebView()
        {
            if (is64())
            {
                return wkeCreateWebView_x64();
            }

            return wkeCreateWebView_x86();
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeGetWebView", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Ansi)]
        //public static extern IntPtr wkeGetWebView(string name);

        //[DllImport(DLL_x86, EntryPoint = "wkeDestroyWebView", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeDestroyWebView(IntPtr webView);

        //[DllImport(DLL_x86, EntryPoint = "wkeSetMemoryCacheEnable", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeSetMemoryCacheEnable(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);


        [DllImport(DLL_x86, EntryPoint = "wkeOnTitleChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnTitleChanged_x86(IntPtr webView, TitleChangedCallback callback, IntPtr callbackParam);
        [DllImport(DLL_x64, EntryPoint = "wkeOnTitleChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnTitleChanged_x64(IntPtr webView, TitleChangedCallback callback, IntPtr callbackParam);

        public static void wkeOnTitleChanged(IntPtr webView, TitleChangedCallback callback, IntPtr callbackParam)
        {
            if (is64())
            {
                wkeOnTitleChanged_x64(webView, callback, callbackParam);
            }
            else
            {
                wkeOnTitleChanged_x86(webView, callback, callbackParam);
            }
        }


        [DllImport(DLL_x86, EntryPoint = "wkeOnMouseOverUrlChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnMouseOverUrlChanged_x86(IntPtr webView, TitleChangedCallback callback, IntPtr callbackParam);
        [DllImport(DLL_x64, EntryPoint = "wkeOnMouseOverUrlChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnMouseOverUrlChanged_x64(IntPtr webView, TitleChangedCallback callback, IntPtr callbackParam);

        public static void wkeOnMouseOverUrlChanged(IntPtr webView, TitleChangedCallback callback, IntPtr callbackParam)
        {
            if (is64())
            {
                wkeOnMouseOverUrlChanged_x64(webView, callback, callbackParam);
            }
            else
            {
                wkeOnMouseOverUrlChanged_x86(webView, callback, callbackParam);
            }
        }


        [DllImport(DLL_x86, EntryPoint = "wkeSetNavigationToNewWindowEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetNavigationToNewWindowEnable_x86(IntPtr webView,
            [MarshalAs(UnmanagedType.I1)] bool b);

        [DllImport(DLL_x64, EntryPoint = "wkeSetNavigationToNewWindowEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetNavigationToNewWindowEnable_x64(IntPtr webView,
            [MarshalAs(UnmanagedType.I1)] bool b);

        public static void wkeSetNavigationToNewWindowEnable(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetNavigationToNewWindowEnable_x64(webView, enable);
            }
            else
            {
                wkeSetNavigationToNewWindowEnable_x86(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetNpapiPluginsEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetNpapiPluginsEnabled_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(DLL_x64, EntryPoint = "wkeSetNpapiPluginsEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetNpapiPluginsEnabled_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);

        public static void wkeSetNpapiPluginsEnabled(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetNpapiPluginsEnabled_x64(webView, enable);
            }
            else
            {
                wkeSetNpapiPluginsEnabled_x86(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetHeadlessEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetHeadlessEnabled_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(DLL_x64, EntryPoint = "wkeSetHeadlessEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetHeadlessEnabled_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);

        public static void wkeSetHeadlessEnabled(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetHeadlessEnabled_x64(webView, enable);
            }
            else
            {
                wkeSetHeadlessEnabled_x86(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetCspCheckEnable", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetCspCheckEnable_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(DLL_x64, EntryPoint = "wkeSetCspCheckEnable", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetCspCheckEnable_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);

        public static void wkeSetCspCheckEnable(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetCspCheckEnable_x64(webView, enable);
            }
            else
            {
                wkeSetCspCheckEnable_x86(webView, enable);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeSetProxy", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void wkeSetProxy_x86(WKEProxy proxy);
        //[DllImport(DLL_x64, EntryPoint = "wkeSetProxy", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void wkeSetProxy_x64(WKEProxy proxy);

        //public static void wkeSetProxy(WKEProxy proxy)
        //{
        //    if (is64())
        //    {
        //        wkeSetProxy_x64(proxy);
        //    }
        //    else
        //    {
        //        wkeSetProxy_x86(proxy);
        //    }
        //}

        [DllImport(DLL_x86, EntryPoint = "wkeSetViewProxy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetViewProxy_x86(IntPtr webView, ref WKEProxy proxy);
        [DllImport(DLL_x64, EntryPoint = "wkeSetViewProxy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetViewProxy_x64(IntPtr webView, ref WKEProxy proxy);

        public static void wkeSetViewProxy(IntPtr webView, WKEProxy proxy)
        {
            if (is64())
            {
                wkeSetViewProxy_x64(webView, ref proxy);
            }
            else
            {
                wkeSetViewProxy_x86(webView, ref proxy);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetHandle", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetHandle_x86(IntPtr webView, IntPtr handle);
        [DllImport(DLL_x64, EntryPoint = "wkeSetHandle", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetHandle_x64(IntPtr webView, IntPtr handle);

        public static void wkeSetHandle(IntPtr webView, IntPtr handle)
        {
            if (is64())
            {
                wkeSetHandle_x64(webView, handle);
            }
            else
            {
                wkeSetHandle_x86(webView, handle);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeSetHandleOffset", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeSetHandleOffset(IntPtr webView, int x, int y);

        //[DllImport(DLL_x86, EntryPoint = "wkeIsTransparent", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte wkeIsTransparent(IntPtr webView);

        [DllImport(DLL_x86, EntryPoint = "wkeSetTransparent", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetTransparent_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(DLL_x64, EntryPoint = "wkeSetTransparent", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetTransparent_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);

        public static void wkeSetTransparent(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetTransparent_x64(webView, enable);
            }
            else
            {
                wkeSetTransparent_x86(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetDragEnable", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetDragEnable_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(DLL_x64, EntryPoint = "wkeSetDragEnable", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetDragEnable_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);

        public static void wkeSetDragEnable(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetDragEnable_x64(webView, enable);
            }
            else
            {
                wkeSetDragEnable_x86(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetDragDropEnable", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetDragDropEnable_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(DLL_x64, EntryPoint = "wkeSetDragDropEnable", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetDragDropEnable_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);

        public static void wkeSetDragDropEnable(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetDragDropEnable_x64(webView, enable);
            }
            else
            {
                wkeSetDragDropEnable_x86(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetUserAgent", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeSetUserAgent_x86(IntPtr webView, string userAgent);

        [DllImport(DLL_x64, EntryPoint = "wkeSetUserAgent", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeSetUserAgent_x64(IntPtr webView, string userAgent);

        public static void wkeSetUserAgent(IntPtr webView, string userAgent)
        {
            if (is64())
            {
                wkeSetUserAgent_x64(webView, userAgent);
            }
            else
            {
                wkeSetUserAgent_x86(webView, userAgent);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetUserAgent", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern IntPtr wkeGetUserAgent_x86(IntPtr webView);

        [DllImport(DLL_x64, EntryPoint = "wkeGetUserAgent", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern IntPtr wkeGetUserAgent_x64(IntPtr webView);

        public static IntPtr wkeGetUserAgent(IntPtr webView)
        {
            return is64() ? wkeGetUserAgent_x64(webView) : wkeGetUserAgent_x86(webView);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeLoadURL", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeLoadURL_x86(IntPtr webView, string url);

        [DllImport(DLL_x64, EntryPoint = "wkeLoadURL", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeLoadURL_x64(IntPtr webView, string url);

        public static void wkeLoadURL(IntPtr webView, string url)
        {
            if (is64())
            {
                wkeLoadURL_x64(webView, url);
            }
            else
            {
                wkeLoadURL_x86(webView, url);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeLoadHTML", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeLoadHTML_x86(IntPtr webView, string html);

        [DllImport(DLL_x64, EntryPoint = "wkeLoadHTML", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeLoadHTML_x64(IntPtr webView, string html);

        public static void wkeLoadHTML(IntPtr webView, string html)
        {
            if (is64())
            {
                wkeLoadHTML_x64(webView, html);
            }
            else
            {
                wkeLoadHTML_x86(webView, html);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeLoadHtmlWithBaseUrl", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeLoadHtmlWithBaseUrl_x86(IntPtr webView, string html, string baseUrl);

        [DllImport(DLL_x64, EntryPoint = "wkeLoadHtmlWithBaseUrl", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeLoadHtmlWithBaseUrl_x64(IntPtr webView, string html, string baseUrl);

        public static void wkeLoadHtmlWithBaseUrl(IntPtr webView, string html, string baseUrl)
        {
            if (is64())
            {
                wkeLoadHtmlWithBaseUrl_x64(webView, html, baseUrl);
            }
            else
            {
                wkeLoadHtmlWithBaseUrl_x86(webView, html, baseUrl);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeLoadFileW", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeLoadFileW_x86(IntPtr webView, string fileName);

        [DllImport(DLL_x64, EntryPoint = "wkeLoadFileW", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeLoadFileW_x64(IntPtr webView, string fileName);

        public static void wkeLoadFileW(IntPtr webView, string fileName)
        {
            if (is64())
            {
                wkeLoadFileW_x64(webView, fileName);
            }
            else
            {
                wkeLoadFileW_x86(webView, fileName);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetURL", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetURL_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetURL", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetURL_x64(IntPtr webView);

        public static IntPtr wkeGetURL(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetURL_x64(webView);
            }

            return wkeGetURL_x86(webView);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetFrameUrl", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetFrameUrl_x86(IntPtr webView,IntPtr frameId);
        [DllImport(DLL_x64, EntryPoint = "wkeGetFrameUrl", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetFrameUrl_x64(IntPtr webView, IntPtr frameId);

        public static IntPtr wkeGetFrameUrl(IntPtr webView, IntPtr frameId)
        {
            if (is64())
            {
                return wkeGetFrameUrl_x64(webView, frameId);
            }

            return wkeGetFrameUrl_x86(webView, frameId);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeRunJsByFrame", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern long wkeRunJsByFrame_x86(IntPtr webView, IntPtr frameId, string str,
            [MarshalAs(UnmanagedType.I1)] bool isInClosure);

        [DllImport(DLL_x64, EntryPoint = "wkeRunJsByFrame", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern long wkeRunJsByFrame_x64(IntPtr webView, IntPtr frameId, string str,
            [MarshalAs(UnmanagedType.I1)] bool isInClosure);

        public static long wkeRunJsByFrame(IntPtr webView, IntPtr frameId, string str, bool isInClosure)
        {
            return is64() ? 
                wkeRunJsByFrame_x64(webView, frameId, str, isInClosure) : 
                wkeRunJsByFrame_x86(webView, frameId, str, isInClosure);
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeIsLoading", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte wkeIsLoading(IntPtr webView);

        [DllImport(DLL_x86, EntryPoint = "wkeIsDocumentReady", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsDocumentReady_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeIsDocumentReady", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsDocumentReady_x64(IntPtr webView);

        public static bool wkeIsDocumentReady(IntPtr webView)
        {
            return (is64() ? wkeIsDocumentReady_x64(webView) : wkeIsDocumentReady_x86(webView)) == 1;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeStopLoading", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeStopLoading_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeStopLoading", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeStopLoading_x64(IntPtr webView);

        public static void wkeStopLoading(IntPtr webView)
        {
            if (is64())
            {
                wkeStopLoading_x64(webView);
            }
            else
            {
                wkeStopLoading_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeReload", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeReload_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeReload", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeReload_x64(IntPtr webView);

        public static void wkeReload(IntPtr webView)
        {
            if (is64())
            {
                wkeReload_x64(webView);
            }
            else
            {
                wkeReload_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetTitle_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetTitle_x64(IntPtr webView);

        public static IntPtr wkeGetTitle(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetTitle_x64(webView);
            }

            return wkeGetTitle_x86(webView);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeResize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeResize_x86(IntPtr webView, int w, int h);
        [DllImport(DLL_x64, EntryPoint = "wkeResize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeResize_x64(IntPtr webView, int w, int h);

        public static void wkeResize(IntPtr webView, int w, int h)
        {
            if (is64())
            {
                wkeResize_x64(webView, w, h);
            }
            else
            {
                wkeResize_x86(webView, w, h);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern int wkeGetWidth_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern int wkeGetWidth_x64(IntPtr webView);

        public static int wkeGetWidth(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetWidth_x64(webView);
            }

            return wkeGetWidth_x86(webView);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern int wkeGetHeight_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern int wkeGetHeight_x64(IntPtr webView);

        public static int wkeGetHeight(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetHeight_x64(webView);
            }

            return wkeGetHeight_x86(webView);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetContentWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern int wkeGetContentWidth_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetContentWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern int wkeGetContentWidth_x64(IntPtr webView);

        public static int wkeGetContentWidth(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetContentWidth_x64(webView);
            }

            return wkeGetContentWidth_x86(webView);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetContentHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern int wkeGetContentHeight_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetContentHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern int wkeGetContentHeight_x64(IntPtr webView);

        public static int wkeGetContentHeight(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetContentHeight_x64(webView);
            }

            return wkeGetContentHeight_x86(webView);
        }

        //[DllImport(DLL_x86, EntryPoint = "wkePaint2", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkePaint2(IntPtr webView, IntPtr bits, int bufWid, int bufHei, int xDst, int yDst,
        //    int w, int h, int xSrc, int ySrc, [MarshalAs(UnmanagedType.I1)] bool bCopyAlpha);

        [DllImport(DLL_x86, EntryPoint = "wkePaint", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaint(IntPtr webView, IntPtr bits, byte pitch);

        [DllImport(DLL_x86, EntryPoint = "wkeGetViewDC", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetViewDC_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetViewDC", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetViewDC_x64(IntPtr webView);

        public static IntPtr wkeGetViewDC(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetViewDC_x64(webView);
            }

            return wkeGetViewDC_x86(webView);
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeGetHostHWND", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr wkeGetHostHWND(IntPtr webView);

        [DllImport(DLL_x86, EntryPoint = "wkeCanGoBack", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeCanGoBack_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeCanGoBack", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeCanGoBack_x64(IntPtr webView);

        public static bool wkeCanGoBack(IntPtr webView)
        {
            if (is64())
            {
                return wkeCanGoBack_x64(webView) != 0;
            }

            return wkeCanGoBack_x86(webView) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGoBack", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeGoBack_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGoBack", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeGoBack_x64(IntPtr webView);

        public static bool wkeGoBack(IntPtr webView)
        {
            if (is64())
            {
                return wkeGoBack_x64(webView) != 0;
            }
            else
            {
                return wkeGoBack_x86(webView) != 0;
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeCanGoForward", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeCanGoForward_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeCanGoForward", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeCanGoForward_x64(IntPtr webView);

        public static bool wkeCanGoForward(IntPtr webView)
        {
            if (is64())
            {
                return wkeCanGoForward_x64(webView) != 0;
            }

            return wkeCanGoForward_x86(webView) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGoForward", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeGoForward_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGoForward", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeGoForward_x64(IntPtr webView);

        public static bool wkeGoForward(IntPtr webView)
        {
            if (is64())
            {
                return wkeGoForward_x64(webView) != 0;
            }

            return wkeGoForward_x86(webView) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeEditorSelectAll", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeEditorSelectAll_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeEditorSelectAll", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeEditorSelectAll_x64(IntPtr webView);

        public static void wkeEditorSelectAll(IntPtr webView)
        {
            if (is64())
            {
                wkeEditorSelectAll_x64(webView);
            }
            else
            {
                wkeEditorSelectAll_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeEditorUnSelect", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeEditorUnSelect_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeEditorUnSelect", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeEditorUnSelect_x64(IntPtr webView);

        public static void wkeEditorUnSelect(IntPtr webView)
        {
            if (is64())
            {
                wkeEditorUnSelect_x64(webView);
            }
            else
            {
                wkeEditorUnSelect_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeEditorCopy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorCopy_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeEditorCopy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorCopy_x64(IntPtr webView);

        public static void wkeEditorCopy(IntPtr webView)
        {
            if (is64())
            {
                wkeEditorCopy_x64(webView);
            }
            else
            {
                wkeEditorCopy_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeEditorCut", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorCut_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeEditorCut", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorCut_x64(IntPtr webView);

        public static void wkeEditorCut(IntPtr webView)
        {
            if (is64())
            {
                wkeEditorCut_x64(webView);
            }
            else
            {
                wkeEditorCut_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeEditorPaste", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorPaste_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeEditorPaste", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorPaste_x64(IntPtr webView);

        public static void wkeEditorPaste(IntPtr webView)
        {
            if (is64())
            {
                wkeEditorPaste_x64(webView);
            }
            else
            {
                wkeEditorPaste_x64(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeEditorDelete", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorDelete_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeEditorDelete", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorDelete_x64(IntPtr webView);

        public static void wkeEditorDelete(IntPtr webView)
        {
            if (is64())
            {
                wkeEditorDelete_x64(webView);
            }
            else
            {
                wkeEditorDelete_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeEditorUndo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorUndo_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeEditorUndo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorUndo_x64(IntPtr webView);

        public static void wkeEditorUndo(IntPtr webView)
        {
            if (is64())
            {
                wkeEditorUndo_x64(webView);
            }
            else
            {
                wkeEditorUndo_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeEditorRedo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorRedo_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeEditorRedo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeEditorRedo_x64(IntPtr webView);

        public static void wkeEditorRedo(IntPtr webView)
        {
            if (is64())
            {
                wkeEditorRedo_x64(webView);
            }
            else
            {
                wkeEditorRedo_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetCookieW", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetCookie_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetCookieW", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetCookie_x64(IntPtr webView);

        public static IntPtr wkeGetCookie(IntPtr webView)
        {
            return is64() ? wkeGetCookie_x64(webView) : wkeGetCookie_x86(webView);
        }


        [DllImport(DLL_x86, EntryPoint = "wkePerformCookieCommand", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkePerformCookieCommand_x86(IntPtr webView, wkeCookieCommand command);
        [DllImport(DLL_x64, EntryPoint = "wkePerformCookieCommand", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkePerformCookieCommand_x64(IntPtr webView, wkeCookieCommand command);

        public static void wkePerformCookieCommand(IntPtr webView, wkeCookieCommand command)
        {
            if (is64())
            {
                wkePerformCookieCommand_x64(webView, command);
            }
            else
            {
                wkePerformCookieCommand_x86(webView, command);
            }
        }


        [DllImport(DLL_x86, EntryPoint = "wkeSetCookieEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetCookieEnabled_x86(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(DLL_x64, EntryPoint = "wkeSetCookieEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetCookieEnabled_x64(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);

        public static void wkeSetCookieEnabled(IntPtr webView, bool enable)
        {
            if (is64())
            {
                wkeSetCookieEnabled_x64(webView, enable);
            }
            else
            {
                wkeSetCookieEnabled_x86(webView, enable);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeIsCookieEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsCookieEnabled_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeIsCookieEnabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsCookieEnabled_x64(IntPtr webView);

        public static bool wkeIsCookieEnabled(IntPtr webView)
        {
            if (is64())
            {
                return wkeIsCookieEnabled_x64(webView) != 0;
            }

            return wkeIsCookieEnabled_x86(webView) != 0;
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeSetCookieJarPath", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Unicode)]
        //public static extern void wkeSetCookieJarPath(IntPtr webView, string path);

        //[DllImport(DLL_x86, EntryPoint = "wkeSetCookieJarFullPath", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Unicode)]
        //public static extern void wkeSetCookieJarFullPath(IntPtr webView, string path);

        //[DllImport(DLL_x86, EntryPoint = "wkeSetMediaVolume", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeSetMediaVolume(IntPtr webView, float volume);

        //[DllImport(DLL_x86, EntryPoint = "wkeGetMediaVolume", CallingConvention = CallingConvention.Cdecl)]
        //public static extern float wkeGetMediaVolume(IntPtr webView);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(DLL_x86, EntryPoint = "wkeFireMouseEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireMouseEvent_x86(IntPtr webView, uint message, int x, int y, uint flags);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(DLL_x64, EntryPoint = "wkeFireMouseEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireMouseEvent_x64(IntPtr webView, uint message, int x, int y, uint flags);

        public static bool wkeFireMouseEvent(IntPtr webView, uint message, int x, int y, uint flags)
        {
            if (is64())
            {
                return wkeFireMouseEvent_x64(webView, message, x, y, flags) != 0;
            }
            return wkeFireMouseEvent_x86(webView, message, x, y, flags) != 0;
        }





        [DllImport(DLL_x86, EntryPoint = "wkeFireContextMenuEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireContextMenuEvent_x86(IntPtr webView, int x, int y, uint flags);
        [DllImport(DLL_x64, EntryPoint = "wkeFireContextMenuEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireContextMenuEvent_x64(IntPtr webView, int x, int y, uint flags);

        public static bool wkeFireContextMenuEvent(IntPtr webView, int x, int y, uint flags)
        {
            if (is64())
            {
                return wkeFireContextMenuEvent_x64(webView, x, y, flags) != 0;
            }
            return wkeFireContextMenuEvent_x86(webView, x, y, flags) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeFireMouseWheelEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireMouseWheelEvent_x86(IntPtr webView, int x, int y, int delta, uint flags);
        [DllImport(DLL_x64, EntryPoint = "wkeFireMouseWheelEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireMouseWheelEvent_x64(IntPtr webView, int x, int y, int delta, uint flags);

        public static bool wkeFireMouseWheelEvent(IntPtr webView, int x, int y, int delta, uint flags)
        {
            if (is64())
            {
                return wkeFireMouseWheelEvent_x64(webView, x, y, delta, flags) != 0;
            }
            return wkeFireMouseWheelEvent_x86(webView, x, y, delta, flags) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeFireKeyUpEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireKeyUpEvent_x86(IntPtr webView, int virtualKeyCode, uint flags,
            [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport(DLL_x64, EntryPoint = "wkeFireKeyUpEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireKeyUpEvent_x64(IntPtr webView, int virtualKeyCode, uint flags,
            [MarshalAs(UnmanagedType.I1)] bool systemKey);

        public static bool wkeFireKeyUpEvent(IntPtr webView, int virtualKeyCode, uint flags, bool systemKey)
        {
            if (is64())
            {
                return wkeFireKeyUpEvent_x64(webView, virtualKeyCode, flags, systemKey) != 0;
            }

            return wkeFireKeyUpEvent_x86(webView, virtualKeyCode, flags, systemKey) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeFireKeyDownEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireKeyDownEvent_x86(IntPtr webView, int virtualKeyCode, uint flags,
            [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport(DLL_x64, EntryPoint = "wkeFireKeyDownEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireKeyDownEvent_x64(IntPtr webView, int virtualKeyCode, uint flags,
            [MarshalAs(UnmanagedType.I1)] bool systemKey);

        public static bool wkeFireKeyDownEvent(IntPtr webView, int virtualKeyCode, uint flags, bool systemKey)
        {
            if (is64())
            {
                return wkeFireKeyDownEvent_x64(webView, virtualKeyCode, flags, systemKey) != 0;
            }

            return wkeFireKeyDownEvent_x86(webView, virtualKeyCode, flags, systemKey) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeFireKeyPressEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireKeyPressEvent_x86(IntPtr webView, int charCode, uint flags,
            [MarshalAs(UnmanagedType.I1)] bool systemKey);
        [DllImport(DLL_x64, EntryPoint = "wkeFireKeyPressEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireKeyPressEvent_x64(IntPtr webView, int charCode, uint flags,
            [MarshalAs(UnmanagedType.I1)] bool systemKey);

        public static bool wkeFireKeyPressEvent(IntPtr webView, int charCode, uint flags, bool systemKey)
        {
            if (is64())
            {
                return wkeFireKeyPressEvent_x64(webView, charCode, flags, systemKey) != 0;
            }

            return wkeFireKeyPressEvent_x86(webView, charCode, flags, systemKey) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeFireWindowsMessage", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireWindowsMessage_x86(IntPtr webView, IntPtr hWnd, uint message, IntPtr wParam,
            IntPtr lParam, IntPtr result);
        [DllImport(DLL_x64, EntryPoint = "wkeFireWindowsMessage", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeFireWindowsMessage_x64(IntPtr webView, IntPtr hWnd, uint message, IntPtr wParam,
            IntPtr lParam, IntPtr result);

        public static bool wkeFireWindowsMessage(IntPtr webView, IntPtr hWnd, uint message, IntPtr wParam,
            IntPtr lParam, IntPtr result)
        {
            if (is64())
            {
                return wkeFireWindowsMessage_x64(webView, hWnd, message, wParam, lParam, result) != 0;
            }

            return wkeFireWindowsMessage_x86(webView, hWnd, message, wParam, lParam, result) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeSetFocus_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeSetFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeSetFocus_x64(IntPtr webView);

        public static bool wkeSetFocus(IntPtr webView)
        {
            if (is64())
            {
                return wkeSetFocus_x64(webView) != 0;
            }

            return wkeSetFocus_x86(webView) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeKillFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeKillFocus_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeKillFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeKillFocus_x64(IntPtr webView);

        public static bool wkeKillFocus(IntPtr webView)
        {
            if (is64())
            {
                return wkeKillFocus_x64(webView) != 0;
            }
            return wkeKillFocus_x86(webView) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetCaretRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern wkeRect wkeGetCaretRect_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetCaretRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern wkeRect wkeGetCaretRect_x64(IntPtr webView);

        public static wkeRect wkeGetCaretRect(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetCaretRect_x64(webView);
            }

            return wkeGetCaretRect_x86(webView);
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeRunJSW", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Unicode)]
        //public static extern long wkeRunJSW(IntPtr webView, string script);

        [DllImport(DLL_x86, EntryPoint = "wkeGlobalExec", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGlobalExec_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGlobalExec", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGlobalExec_x64(IntPtr webView);

        public static IntPtr wkeGlobalExec(IntPtr webView)
        {
            if (is64())
            {
                return wkeGlobalExec_x64(webView);
            }

            return wkeGlobalExec_x86(webView);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeSetZoomFactor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetZoomFactor_x86(IntPtr webView, float factor);
        [DllImport(DLL_x64, EntryPoint = "wkeSetZoomFactor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeSetZoomFactor_x64(IntPtr webView, float factor);

        public static void wkeSetZoomFactor(IntPtr webView, float factor)
        {
            if (is64())
            {
                wkeSetZoomFactor_x64(webView, factor);
            }
            else
            {
                wkeSetZoomFactor_x86(webView, factor);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetZoomFactor", CallingConvention = CallingConvention.Cdecl)]
        private static extern float wkeGetZoomFactor_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetZoomFactor", CallingConvention = CallingConvention.Cdecl)]
        private static extern float wkeGetZoomFactor_x64(IntPtr webView);

        public static float wkeGetZoomFactor(IntPtr webView)
        {
            if (is64())
            {
                return wkeGetZoomFactor_x64(webView);
            }
            else
            {
                return wkeGetZoomFactor_x86(webView);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetString", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetString_x86(IntPtr wkeString);
        [DllImport(DLL_x64, EntryPoint = "wkeGetString", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetString_x64(IntPtr wkeString);

        public static IntPtr wkeGetString(IntPtr wkeString)
        {
            if (is64())
            {
                return wkeGetString_x64(wkeString);
            }

            return wkeGetString_x86(wkeString);
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeGetStringW", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr wkeGetStringW(IntPtr wkeString);

        //[DllImport(DLL_x86, EntryPoint = "wkeSetStringW", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Unicode)]
        //public static extern void wkeSetStringW(IntPtr wkeString, string str, int len);

        //[DllImport(DLL_x86, EntryPoint = "wkeCreateStringW", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Unicode)]
        //public static extern IntPtr wkeCreateStringW(string str, int len);

        //[DllImport(DLL_x86, EntryPoint = "wkeDeleteString", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeDeleteString(IntPtr wkeString);

        //[DllImport(DLL_x86, EntryPoint = "wkeSetUserKeyValue", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Ansi)]
        //public static extern void wkeSetUserKeyValue(IntPtr webView, string key, IntPtr value);

        //[DllImport(DLL_x86, EntryPoint = "wkeGetUserKeyValue", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Ansi)]
        //public static extern IntPtr wkeGetUserKeyValue(IntPtr webView, string key);

        [DllImport(DLL_x86, EntryPoint = "wkeGetCursorInfoType", CallingConvention = CallingConvention.Cdecl)]
        private static extern wkeCursorInfo wkeGetCursorInfoType_x86(IntPtr webView);
        [DllImport(DLL_x64, EntryPoint = "wkeGetCursorInfoType", CallingConvention = CallingConvention.Cdecl)]
        private static extern wkeCursorInfo wkeGetCursorInfoType_x64(IntPtr webView);

        public static wkeCursorInfo wkeGetCursorInfoType(IntPtr webView)
        {
            return is64() ? wkeGetCursorInfoType_x64(webView) : wkeGetCursorInfoType_x86(webView);
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeSetDragFiles", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeSetDragFiles(IntPtr webView, IntPtr clintPos, IntPtr screenPos,
        //    [MarshalAs(UnmanagedType.LPArray)] IntPtr[] files, int filesCount);

        //[DllImport(DLL_x86, EntryPoint = "wkeOnTitleChanged", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeOnTitleChanged(IntPtr webView, wkeTitleChangedCallback callback, IntPtr param);

        //[DllImport(DLL_x86, EntryPoint = "wkeOnURLChanged", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeOnURLChanged(IntPtr webView, wkeURLChangedCallback callback, IntPtr param);

        [DllImport(DLL_x86, EntryPoint = "wkeOnURLChanged2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnURLChanged2_x86(IntPtr webView, wkeURLChangedCallback2 callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnURLChanged2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnURLChanged2_x64(IntPtr webView, wkeURLChangedCallback2 callback, IntPtr param);

        public static void wkeOnURLChanged2(IntPtr webView, wkeURLChangedCallback2 callback, IntPtr param)
        {
            if (is64())
            {
                wkeOnURLChanged2_x64(webView, callback, param);
            }
            else
            {
                wkeOnURLChanged2_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeOnPaintUpdated", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnPaintUpdated_x86(IntPtr webView, wkePaintUpdatedCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnPaintUpdated", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnPaintUpdated_x64(IntPtr webView, wkePaintUpdatedCallback callback, IntPtr param);


        [DllImport(DLL_x86, EntryPoint = "wkeOnPaintBitUpdated", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnPaintBitUpdated_x86(IntPtr webView, wkePaintUpdatedCallback callback, IntPtr param);

        [DllImport(DLL_x64, EntryPoint = "wkeOnPaintBitUpdated", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnPaintBitUpdated_x64(IntPtr webView, wkePaintUpdatedCallback callback, IntPtr param);

        //void wkeOnPaintBitUpdated(wkeWebView webView, wkePaintBitUpdatedCallback callback, void* callbackParam)

        public static void wkeOnPaintBitUpdated(IntPtr webView, wkePaintUpdatedCallback callback)
        {
            if (is64())
            {
                wkeOnPaintBitUpdated_x64(webView, callback, IntPtr.Zero);
            }
            else
            {
                wkeOnPaintBitUpdated_x86(webView, callback, IntPtr.Zero);
            }
        }

        public static void wkeOnPaintUpdated(IntPtr webView, wkePaintUpdatedCallback callback)
        {
            if (is64())
            {
                wkeOnPaintUpdated_x64(webView, callback, IntPtr.Zero);
            }
            else
            {
                wkeOnPaintUpdated_x86(webView, callback, IntPtr.Zero);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeOnAlertBox", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void wkeOnAlertBox_x86(IntPtr webView, wkeAlertBoxCallback callback, IntPtr param);
        //[DllImport(DLL_x64, EntryPoint = "wkeOnAlertBox", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void wkeOnAlertBox_x64(IntPtr webView, wkeAlertBoxCallback callback, IntPtr param);

        //public static void wkeOnAlertBox(IntPtr webView, wkeAlertBoxCallback callback, IntPtr param)
        //{
        //    if (is64())
        //    {
        //        wkeOnAlertBox_x64(webView, callback, param);
        //    }
        //    else
        //    {
        //        wkeOnAlertBox_x86(webView, callback, param);
        //    }
        //}

        //[DllImport(DLL_x86, EntryPoint = "wkeOnConfirmBox", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void wkeOnConfirmBox_x86(IntPtr webView, wkeConfirmBoxCallback callback, IntPtr param);
        //[DllImport(DLL_x86, EntryPoint = "wkeOnConfirmBox", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void wkeOnConfirmBox_x64(IntPtr webView, wkeConfirmBoxCallback callback, IntPtr param);

        //public static void wkeOnConfirmBox(IntPtr webView, wkeConfirmBoxCallback callback, IntPtr param)
        //{
        //    if (is64())
        //    {
        //        wkeOnConfirmBox_x64(webView, callback, param);
        //    }
        //    else
        //    {
        //        wkeOnConfirmBox_x86(webView, callback, param);
        //    }
        //}

        //[DllImport(DLL_x86, EntryPoint = "wkeOnPromptBox", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeOnPromptBox(IntPtr webView, wkePromptBoxCallback callback, IntPtr param);

        [DllImport(DLL_x86, EntryPoint = "wkeOnNavigation", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnNavigation_x86(IntPtr webView, wkeNavigationCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnNavigation", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnNavigation_x64(IntPtr webView, wkeNavigationCallback callback, IntPtr param);

        public static void wkeOnNavigation(IntPtr webView, wkeNavigationCallback callback, IntPtr param)
        {
            if (is64())
            {
                wkeOnNavigation_x64(webView, callback, param);
            }
            else
            {
                wkeOnNavigation_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeOnCreateView", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnCreateView_x86(IntPtr webView, wkeCreateViewCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnCreateView", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnCreateView_x64(IntPtr webView, wkeCreateViewCallback callback, IntPtr param);

        public static void wkeOnCreateView(IntPtr webView, wkeCreateViewCallback callback, IntPtr param)
        {
            if (is64())
            {
                wkeOnCreateView_x64(webView, callback, param);
            }
            else
            {
                wkeOnCreateView_x86(webView, callback, param);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeOnDocumentReady", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeOnDocumentReady(IntPtr webView, wkeDocumentReadyCallback callback, IntPtr param);

        [DllImport(DLL_x86, EntryPoint = "wkeOnDocumentReady2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnDocumentReady2_x86(IntPtr webView, wkeDocumentReady2Callback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnDocumentReady2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnDocumentReady2_x64(IntPtr webView, wkeDocumentReady2Callback callback, IntPtr param);

        public static void wkeOnDocumentReady2(IntPtr webView, wkeDocumentReady2Callback callback, IntPtr param)
        {
            if (is64())
            {
                wkeOnDocumentReady2_x64(webView, callback, param);
            }
            else
            {
                wkeOnDocumentReady2_x86(webView, callback, param);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeOnLoadingFinish", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeOnLoadingFinish(IntPtr webView, wkeLoadingFinishCallback callback, IntPtr param);

        [DllImport(DLL_x86, EntryPoint = "wkeOnDownload", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnDownload_x86(IntPtr webView, wkeDownloadCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnDownload", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnDownload_x64(IntPtr webView, wkeDownloadCallback callback, IntPtr param);

        public static void wkeOnDownload(IntPtr webView, wkeDownloadCallback callback, IntPtr param)
        {
            if (is64())
            {
                wkeOnDownload_x64(webView, callback, param);
            }
            else
            {
                wkeOnDownload_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeOnConsole", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnConsole_x86(IntPtr webView, wkeConsoleCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnConsole", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnConsole_x64(IntPtr webView, wkeConsoleCallback callback, IntPtr param);

        public static void wkeOnConsole(IntPtr webView, wkeConsoleCallback callback, IntPtr param)
        {
            if (is64())
            {
                wkeOnConsole_x64(webView, callback, param);
            }
            else
            {
                wkeOnConsole_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetOnResponse", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetOnResponse_x86(IntPtr webView, wkeNetResponseCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeNetOnResponse", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetOnResponse_x64(IntPtr webView, wkeNetResponseCallback callback, IntPtr param);

        public static void wkeNetOnResponse(IntPtr webView, wkeNetResponseCallback callback, IntPtr param)
        {
            if (is64())
            {
                wkeNetOnResponse_x64(webView, callback, param);
            }
            else
            {
                wkeNetOnResponse_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeOnLoadUrlBegin", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnLoadUrlBegin_x86(IntPtr webView, wkeLoadUrlBeginCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnLoadUrlBegin", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnLoadUrlBegin_x64(IntPtr webView, wkeLoadUrlBeginCallback callback, IntPtr param);

        public static void wkeOnLoadUrlBegin(IntPtr webView, wkeLoadUrlBeginCallback callback, IntPtr param)
        {
            if (is64())
            {
                wkeOnLoadUrlBegin_x64(webView, callback, param);
            }
            else
            {
                wkeOnLoadUrlBegin_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeOnLoadUrlEnd", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnLoadUrlEnd_x86(IntPtr webView, wkeLoadUrlEndCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnLoadUrlEnd", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnLoadUrlEnd_x64(IntPtr webView, wkeLoadUrlEndCallback callback, IntPtr param);

        public static void wkeOnLoadUrlEnd(IntPtr webView, wkeLoadUrlEndCallback callback, IntPtr param)
        {
            if (is64())
            {
                wkeOnLoadUrlEnd_x64(webView, callback, param);
            }
            else
            {
                wkeOnLoadUrlEnd_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeOnDidCreateScriptContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnDidCreateScriptContext_x86(IntPtr webView,
            wkeDidCreateScriptContextCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnDidCreateScriptContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnDidCreateScriptContext_x64(IntPtr webView,
            wkeDidCreateScriptContextCallback callback, IntPtr param);

        public static void wkeOnDidCreateScriptContext(IntPtr webView, wkeDidCreateScriptContextCallback callback,
            IntPtr param)
        {
            if (is64())
            {
                wkeOnDidCreateScriptContext_x64(webView, callback, param);
            }
            else
            {
                wkeOnDidCreateScriptContext_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeOnWillReleaseScriptContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnWillReleaseScriptContext_x86(IntPtr webView,
            wkeWillReleaseScriptContextCallback callback, IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeOnWillReleaseScriptContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeOnWillReleaseScriptContext_x64(IntPtr webView,
            wkeWillReleaseScriptContextCallback callback, IntPtr param);

        public static void wkeOnWillReleaseScriptContext(IntPtr webView, wkeWillReleaseScriptContextCallback callback,
            IntPtr param)
        {
            if (is64())
            {
                wkeOnWillReleaseScriptContext_x64(webView, callback, param);
            }
            else
            {
                wkeOnWillReleaseScriptContext_x86(webView, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetSetMIMEType", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void wkeNetSetMIMEType_x86(IntPtr job, string type);

        [DllImport(DLL_x64, EntryPoint = "wkeNetSetMIMEType", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void wkeNetSetMIMEType_x64(IntPtr job, string type);

        public static void wkeNetSetMIMEType(IntPtr job, string type)
        {
            if (is64())
            {
                wkeNetSetMIMEType_x64(job, type);
            }
            else
            {
                wkeNetSetMIMEType_x86(job, type);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeNetSetHTTPHeaderField", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Unicode)]
        //public static extern void wkeNetSetHTTPHeaderField(IntPtr job, string key, string value,
        //    [MarshalAs(UnmanagedType.I1)] bool response);

        [DllImport(DLL_x86, EntryPoint = "wkeNetSetData", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void wkeNetSetData_x86(IntPtr job, [MarshalAs(UnmanagedType.LPArray)] byte[] buf, int len);

        [DllImport(DLL_x64, EntryPoint = "wkeNetSetData", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void wkeNetSetData_x64(IntPtr job, [MarshalAs(UnmanagedType.LPArray)] byte[] buf, int len);

        public static void wkeNetSetData(IntPtr job, byte[] buf, int len)
        {
            if (is64())
            {
                wkeNetSetData_x64(job, buf, len);
            }
            else
            {
                wkeNetSetData_x86(job, buf, len);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetHookRequest", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetHookRequest_x86(IntPtr job);
        [DllImport(DLL_x64, EntryPoint = "wkeNetHookRequest", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetHookRequest_x64(IntPtr job);

        public static void wkeNetHookRequest(IntPtr job)
        {
            if (is64())
            {
                wkeNetHookRequest_x64(job);
            }
            else
            {
                wkeNetHookRequest_x86(job);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetHoldJobToAsynCommit", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetHoldJobToAsynCommit_x86(IntPtr job);
        [DllImport(DLL_x64, EntryPoint = "wkeNetHoldJobToAsynCommit", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetHoldJobToAsynCommit_x64(IntPtr job);

        public static void wkeNetHoldJobToAsynCommit(IntPtr job)
        {
            if (is64())
            {
                wkeNetHoldJobToAsynCommit_x64(job);
            }
            else
            {
                wkeNetHoldJobToAsynCommit_x86(job);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetGetPostBody", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeNetGetPostBody_x86(IntPtr job);
        [DllImport(DLL_x64, EntryPoint = "wkeNetGetPostBody", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeNetGetPostBody_x64(IntPtr job);

        public static IntPtr wkeNetGetPostBody(IntPtr job)
        {
            if (is64())
            {
                return wkeNetGetPostBody_x64(job);
            }
            return wkeNetGetPostBody_x86(job);
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeNetChangeRequestUrl", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Unicode)]
        //public static extern void wkeNetChangeRequestUrl(IntPtr job, string url);

        [DllImport(DLL_x86, EntryPoint = "wkeNetContinueJob", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetContinueJob_x86(IntPtr job);

        [DllImport(DLL_x64, EntryPoint = "wkeNetContinueJob", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetContinueJob_x64(IntPtr job);

        public static void wkeNetContinueJob(IntPtr job)
        {
            if (is64())
            {
                wkeNetContinueJob_x64(job);
            }
            else
            {
                wkeNetContinueJob_x86(job);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetGetRequestMethod", CallingConvention = CallingConvention.Cdecl)]
        private static extern wkeRequestType wkeNetGetRequestMethod_x86(IntPtr job);

        [DllImport(DLL_x64, EntryPoint = "wkeNetGetRequestMethod", CallingConvention = CallingConvention.Cdecl)]
        private static extern wkeRequestType wkeNetGetRequestMethod_x64(IntPtr job);

        public static wkeRequestType wkeNetGetRequestMethod(IntPtr job)
        {
            if (is64())
            {
                return wkeNetGetRequestMethod_x64(job);
            }
            return wkeNetGetRequestMethod_x86(job);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeIsMainFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsMainFrame_x86(IntPtr webview, IntPtr webFrame);
        [DllImport(DLL_x64, EntryPoint = "wkeIsMainFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsMainFrame_x64(IntPtr webview, IntPtr webFrame);

        public static bool wkeIsMainFrame(IntPtr webview, IntPtr frameId)
        {
            if (is64())
            {
                return wkeIsMainFrame_x64(webview, frameId) != 0;
            }

            return wkeIsMainFrame_x86(webview, frameId) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "wkeGetGlobalExecByFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetGlobalExecByFrame_x86(IntPtr webView, IntPtr frameId);
        [DllImport(DLL_x64, EntryPoint = "wkeGetGlobalExecByFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr wkeGetGlobalExecByFrame_x64(IntPtr webView, IntPtr frameId);

        public static IntPtr wkeGetGlobalExecByFrame(IntPtr webView, IntPtr frameId)
        {
            if (is64())
            {
                return wkeGetGlobalExecByFrame_x64(webView, frameId);
            }
            return wkeGetGlobalExecByFrame_x86(webView, frameId);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeIsWebRemoteFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsWebRemoteFrame_x86(IntPtr webView, IntPtr frameId);
        [DllImport(DLL_x64, EntryPoint = "wkeIsWebRemoteFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte wkeIsWebRemoteFrame_x64(IntPtr webView, IntPtr frameId);

        public static bool wkeIsWebRemoteFrame(IntPtr webView, IntPtr frameId)
        {
            if (is64())
            {
                return wkeIsWebRemoteFrame_x64(webView, frameId) != 0;
            }
            return wkeIsWebRemoteFrame_x86(webView, frameId) != 0;
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeWebFrameGetMainFrame", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr wkeWebFrameGetMainFrame(IntPtr webView);

        //[DllImport(DLL_x86, EntryPoint = "wkeWebFrameGetMainWorldScriptContext",
        //    CallingConvention = CallingConvention.Cdecl)]
        //public static extern void wkeWebFrameGetMainWorldScriptContext(IntPtr webFrame, ref IntPtr contextOut);

        //[DllImport(DLL_x86, EntryPoint = "wkeGetWindowHandle", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr wkeGetWindowHandle(IntPtr WebView);

        [DllImport(DLL_x86, EntryPoint = "wkeJsBindFunction", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void wkeJsBindFunction_x86(string name, wkeJsNativeFunction fn, IntPtr param, uint argCount);

        [DllImport(DLL_x64, EntryPoint = "wkeJsBindFunction", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void wkeJsBindFunction_x64(string name, wkeJsNativeFunction fn, IntPtr param, uint argCount);

        public static void wkeJsBindFunction(string name, wkeJsNativeFunction fn, IntPtr param, uint argCount)
        {
            if (is64())
            {
                wkeJsBindFunction_x64(name, fn, param, argCount);
            }
            else
            {
                wkeJsBindFunction_x86(name, fn, param, argCount);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "wkeJsBindGetter", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Ansi)]
        //public static extern void wkeJsBindGetter(string name, wkeJsNativeFunction fn, IntPtr param);

        //[DllImport(DLL_x86, EntryPoint = "wkeJsBindSetter", CallingConvention = CallingConvention.Cdecl,
        //    CharSet = CharSet.Ansi)]
        //public static extern void wkeJsBindSetter(string name, wkeJsNativeFunction fn, IntPtr param);

        [DllImport(DLL_x86, EntryPoint = "jsArgCount", CallingConvention = CallingConvention.Cdecl)]
        private static extern int jsArgCount_x86(IntPtr es);
        [DllImport(DLL_x64, EntryPoint = "jsArgCount", CallingConvention = CallingConvention.Cdecl)]
        private static extern int jsArgCount_x64(IntPtr es);

        public static int jsArgCount(IntPtr es)
        {
            if (is64())
            {
                return jsArgCount_x64(es);
            }

            return jsArgCount_x86(es);
        }

        //[DllImport(DLL_x86, EntryPoint = "jsArgCount", CallingConvention = CallingConvention.Cdecl)]
        //private static extern jsType jsArgType_x86(IntPtr es, int argIdx);
        //[DllImport(DLL_x64, EntryPoint = "jsArgCount", CallingConvention = CallingConvention.Cdecl)]
        //private static extern jsType jsArgType_x64(IntPtr es, int argIdx);

        //public static jsType jsArgType(IntPtr es, int argIdx)
        //{
        //    if (is64())
        //    {
        //        return jsArgType_x64(es, argIdx);
        //    }
        //    return jsArgType_x86(es, argIdx);
        //}

        [DllImport(DLL_x86, EntryPoint = "jsArg", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsArg_x86(IntPtr es, int argIdx);
        [DllImport(DLL_x64, EntryPoint = "jsArg", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsArg_x64(IntPtr es, int argIdx);

        public static long jsArg(IntPtr es, int argIdx)
        {
            if (is64())
            {
                return jsArg_x64(es, argIdx);
            }

            return jsArg_x86(es, argIdx);
        }

        [DllImport(DLL_x86, EntryPoint = "jsGetKeys", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr jsGetKeys_x86(IntPtr es, long value);
        [DllImport(DLL_x64, EntryPoint = "jsGetKeys", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr jsGetKeys_x64(IntPtr es, long value);

        public static IntPtr jsGetKeys(IntPtr es, long value)
        {
            if (is64())
            {
                return jsGetKeys_x64(es, value);
            }

            return jsGetKeys_x86(es, value);
        }

        [DllImport(DLL_x86, EntryPoint = "jsTypeOf", CallingConvention = CallingConvention.Cdecl)]
        private static extern jsType jsTypeOf_x86(long v);
        [DllImport(DLL_x64, EntryPoint = "jsTypeOf", CallingConvention = CallingConvention.Cdecl)]
        private static extern jsType jsTypeOf_x64(long v);

        public static jsType jsTypeOf(long v)
        {
            if (is64())
            {
                return jsTypeOf_x64(v);
            }

            return jsTypeOf_x86(v);
        }

        //[DllImport(DLL_x86, EntryPoint = "jsIsNumber", CallingConvention = CallingConvention.Cdecl)]
        //private static extern byte jsIsNumber_x86(long v);
        //[DllImport(DLL_x64, EntryPoint = "jsIsNumber", CallingConvention = CallingConvention.Cdecl)]
        //private static extern byte jsIsNumber_x64(long v);

        //public static extern bool jsIsNumber(long v)
        //{
        //    if (is64())
        //    {
        //        return jsIsNumber_x64(v) != 0;
        //    }
        //    return jsIsNumber_x86(v) != 0;
        //}

        //[DllImport(DLL_x86, EntryPoint = "jsIsString", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsString(long v);

        //[DllImport(DLL_x86, EntryPoint = "jsIsBoolean", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsBoolean(long v);

        //[DllImport(DLL_x86, EntryPoint = "jsIsObject", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsObject(long v);

        //[DllImport(DLL_x86, EntryPoint = "jsIsFunction", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsFunction(long v);

        //[DllImport(DLL_x86, EntryPoint = "jsIsUndefined", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsUndefined(long v);

        //[DllImport(DLL_x86, EntryPoint = "jsIsNull", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsNull(long v);

        //[DllImport(DLL_x86, EntryPoint = "jsIsArray", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsArray(long v);

        //[DllImport(DLL_x86, EntryPoint = "jsIsTrue", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsTrue(long v);

        //[DllImport(DLL_x86, EntryPoint = "jsIsFalse", CallingConvention = CallingConvention.Cdecl)]
        //public static extern byte jsIsFalse(long v);

        [DllImport(DLL_x86, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsToString(IntPtr es, Int64 v);

        [DllImport(DLL_x86, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsToStringW(IntPtr es, Int64 v);
        [DllImport(DLL_x86, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsStringW(IntPtr es, IntPtr str);

        [DllImport(DLL_x86, EntryPoint = "jsToInt", CallingConvention = CallingConvention.Cdecl)]
        public static extern int jsToInt(IntPtr es, long v);

        [DllImport(DLL_x86, EntryPoint = "jsToFloat", CallingConvention = CallingConvention.Cdecl)]
        public static extern float jsToFloat(IntPtr es, long v);

        [DllImport(DLL_x86, EntryPoint = "jsToDouble", CallingConvention = CallingConvention.Cdecl)]
        private static extern double jsToDouble_x86(IntPtr es, long v);
        [DllImport(DLL_x64, EntryPoint = "jsToDouble", CallingConvention = CallingConvention.Cdecl)]
        private static extern double jsToDouble_x64(IntPtr es, long v);

        public static double jsToDouble(IntPtr es, long v)
        {
            if (is64())
            {
                return jsToDouble_x64(es, v);
            }

            return jsToDouble_x86(es, v);
        }

        [DllImport(DLL_x86, EntryPoint = "jsToBoolean", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte jsToBoolean_x86(IntPtr es, long v);
        [DllImport(DLL_x64, EntryPoint = "jsToBoolean", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte jsToBoolean_x64(IntPtr es, long v);

        public static bool jsToBoolean(IntPtr es, long v)
        {
            if (is64())
            {
                return jsToBoolean_x64(es, v) != 0;
            }

            return jsToBoolean_x86(es, v) != 0;
        }

        [DllImport(DLL_x86, EntryPoint = "jsToTempStringW", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr jsToTempStringW_x86(IntPtr es, long v);
        [DllImport(DLL_x64, EntryPoint = "jsToTempStringW", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr jsToTempStringW_x64(IntPtr es, long v);

        public static IntPtr jsToTempStringW(IntPtr es, long v)
        {
            if (is64())
            {
                return jsToTempStringW_x64(es, v);
            }

            return jsToTempStringW_x86(es, v);
        }

        [DllImport(DLL_x86, EntryPoint = "jsInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsInt_x86(int n);
        [DllImport(DLL_x64, EntryPoint = "jsInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsInt_x64(int n);

        public static long jsInt(int n)
        {
            if (is64())
            {
                return jsInt_x64(n);
            }

            return jsInt_x86(n);
        }

        [DllImport(DLL_x86, EntryPoint = "jsFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsFloat_x86(float f);
        [DllImport(DLL_x64, EntryPoint = "jsFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsFloat_x64(float f);

        public static long jsFloat(float f)
        {
            if (is64())
            {
                return jsFloat_x64(f);
            }

            return jsFloat_x86(f);
        }

        [DllImport(DLL_x86, EntryPoint = "jsDouble", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsDouble_x86(double d);
        [DllImport(DLL_x64, EntryPoint = "jsDouble", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsDouble_x64(double d);

        public static long jsDouble(double d)
        {
            if (is64())
            {
                return jsDouble_x64(d);
            }

            return jsDouble_x86(d);
        }

        [DllImport(DLL_x86, EntryPoint = "jsBoolean", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsBoolean_x86(bool b);
        [DllImport(DLL_x64, EntryPoint = "jsBoolean", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsBoolean_x64(bool b);

        public static long jsBoolean(bool b)
        {
            if (is64())
            {
                return jsBoolean_x64(b);
            }

            return jsBoolean_x86(b);
        }

        [DllImport(DLL_x86, EntryPoint = "jsUndefined", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsUndefined_x86();
        [DllImport(DLL_x64, EntryPoint = "jsUndefined", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsUndefined_x64();

        public static long jsUndefined()
        {
            return is64() ? jsUndefined_x64() : jsUndefined_x86();
        }

        //[DllImport(DLL_x86, EntryPoint = "jsNull", CallingConvention = CallingConvention.Cdecl)]
        //public static extern long jsNull();

        //[DllImport(DLL_x86, EntryPoint = "jsTrue", CallingConvention = CallingConvention.Cdecl)]
        //public static extern long jsTrue();

        //[DllImport(DLL_x86, EntryPoint = "jsFalse", CallingConvention = CallingConvention.Cdecl)]
        //public static extern long jsFalse();

        [DllImport(DLL_x86, EntryPoint = "jsString", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern long jsString_x86(IntPtr es, string str);

        [DllImport(DLL_x64, EntryPoint = "jsString", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern long jsString_x64(IntPtr es, string str);

        public static long jsString(IntPtr es, string str)
        {
            if (is64())
            {
                return jsString_x64(es, str);
            }

            return jsString_x86(es, str);
        }

        [DllImport(DLL_x86, EntryPoint = "jsEmptyObject", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsEmptyObject_x86(IntPtr es);
        [DllImport(DLL_x64, EntryPoint = "jsEmptyObject", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsEmptyObject_x64(IntPtr es);

        public static long jsEmptyObject(IntPtr es)
        {
            if (is64())
            {
                return jsEmptyObject_x64(es);
            }

            return jsEmptyObject_x86(es);
        }

        [DllImport(DLL_x86, EntryPoint = "jsEmptyArray", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsEmptyArray_x86(IntPtr es);
        [DllImport(DLL_x64, EntryPoint = "jsEmptyArray", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsEmptyArray_x64(IntPtr es);

        public static long jsEmptyArray(IntPtr es)
        {
            if (is64())
            {
                return jsEmptyArray_x64(es);
            }

            return jsEmptyArray_x86(es);
        }

        //[DllImport(DLL_x86, EntryPoint = "jsObject", CallingConvention = CallingConvention.Cdecl)]
        //public static extern long jsObject(IntPtr es, IntPtr obj);

        [DllImport(DLL_x86, EntryPoint = "jsFunction", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsFunction_x86(IntPtr es, IntPtr obj);
        [DllImport(DLL_x64, EntryPoint = "jsFunction", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsFunction_x64(IntPtr es, IntPtr obj);

        public static long jsFunction(IntPtr es, IntPtr obj)
        {
            if (is64())
            {
                return jsFunction_x64(es, obj);
            }

            return jsFunction_x86(es, obj);
        }

        //[DllImport(DLL_x86, EntryPoint = "jsGetData", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr jsGetData(IntPtr es, long jsValue);

        [DllImport(DLL_x86, EntryPoint = "jsGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern long jsGet_x86(IntPtr es, long jsValue, string prop);
        [DllImport(DLL_x64, EntryPoint = "jsGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern long jsGet_x64(IntPtr es, long jsValue, string prop);

        public static long jsGet(IntPtr es, long jsValue, string prop)
        {
            if (is64())
            {
                return jsGet_x64(es, jsValue, prop);
            }

            return jsGet_x86(es, jsValue, prop);
        }

        [DllImport(DLL_x86, EntryPoint = "jsSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void jsSet_x86(IntPtr es, long jsValue, string prop, long v);
        [DllImport(DLL_x64, EntryPoint = "jsSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void jsSet_x64(IntPtr es, long jsValue, string prop, long v);

        public static void jsSet(IntPtr es, long jsValue, string prop, long v)
        {
            if (is64())
            {
                jsSet_x64(es, jsValue, prop, v);
            }
            else
            {
                jsSet_x86(es, jsValue, prop, v);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "jsGetAt", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsGetAt_x86(IntPtr es, long jsValue, int index);
        [DllImport(DLL_x64, EntryPoint = "jsGetAt", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsGetAt_x64(IntPtr es, long jsValue, int index);

        public static long jsGetAt(IntPtr es, long jsValue, int index)
        {
            if (is64())
            {
                return jsGetAt_x64(es, jsValue, index);
            }

            return jsGetAt_x86(es, jsValue, index);
        }

        [DllImport(DLL_x86, EntryPoint = "jsSetAt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void jsSetAt_x86(IntPtr es, long jsValue, int index, long v);
        [DllImport(DLL_x64, EntryPoint = "jsSetAt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void jsSetAt_x64(IntPtr es, long jsValue, int index, long v);

        public static void jsSetAt(IntPtr es, long jsValue, int index, long v)
        {
            if (is64())
            {
                jsSetAt_x64(es, jsValue, index, v);
            }
            else
            {
                jsSetAt_x86(es, jsValue, index, v);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "jsGetLength", CallingConvention = CallingConvention.Cdecl)]
        private static extern int jsGetLength_x86(IntPtr es, long jsValue);
        [DllImport(DLL_x64, EntryPoint = "jsGetLength", CallingConvention = CallingConvention.Cdecl)]
        private static extern int jsGetLength_x64(IntPtr es, long jsValue);

        public static int jsGetLength(IntPtr es, long jsValue)
        {
            return is64() ? jsGetLength_x64(es, jsValue) : jsGetLength_x86(es, jsValue);
        }

        [DllImport(DLL_x86, EntryPoint = "jsSetLength", CallingConvention = CallingConvention.Cdecl)]
        private static extern void jsSetLength_x86(IntPtr es, long jsValue, int length);
        [DllImport(DLL_x64, EntryPoint = "jsSetLength", CallingConvention = CallingConvention.Cdecl)]
        private static extern void jsSetLength_x64(IntPtr es, long jsValue, int length);

        public static void jsSetLength(IntPtr es, long jsValue, int length)
        {
            if (is64())
            {
                jsSetLength_x64(es,jsValue,length);
            }
            else
            {
                jsSetLength_x86(es, jsValue, length);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "jsGlobalObject", CallingConvention = CallingConvention.Cdecl)]
        //public static extern long jsGlobalObject(IntPtr es);

        [DllImport(DLL_x86, EntryPoint = "jsGetWebView", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr jsGetWebView_x86(IntPtr es);
        [DllImport(DLL_x64, EntryPoint = "jsGetWebView", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr jsGetWebView_x64(IntPtr es);

        public static IntPtr jsGetWebView(IntPtr es)
        {
            return is64() ? jsGetWebView_x64(es) : jsGetWebView_x86(es);
        }

        //[DllImport(DLL_x86, EntryPoint = "jsEvalW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        //public static extern long jsEvalW(IntPtr es, string str);

        [DllImport(DLL_x86, EntryPoint = "jsEvalExW", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern long jsEvalExW_x86(IntPtr es, string str, [MarshalAs(UnmanagedType.I1)] bool isInClosure);

        [DllImport(DLL_x64, EntryPoint = "jsEvalExW", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern long jsEvalExW_x64(IntPtr es, string str, [MarshalAs(UnmanagedType.I1)] bool isInClosure);

        public static long jsEvalExW(IntPtr es, string str, bool isInClosure)
        {
            return is64() ? jsEvalExW_x64(es, str, isInClosure) : jsEvalExW_x86(es, str, isInClosure);
        }

        [DllImport(DLL_x86, EntryPoint = "jsCall", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsCall_x86(IntPtr es, long func, long thisObject,
            [MarshalAs(UnmanagedType.LPArray)] long[] args, int argCount);

        [DllImport(DLL_x64, EntryPoint = "jsCall", CallingConvention = CallingConvention.Cdecl)]
        private static extern long jsCall_x64(IntPtr es, long func, long thisObject,
            [MarshalAs(UnmanagedType.LPArray)] long[] args, int argCount);

        public static long jsCall(IntPtr es, long func, long thisObject, long[] args, int argCount)
        {
            return is64()
                ? jsCall_x64(es, func, thisObject, args, argCount)
                : jsCall_x86(es, func, thisObject, args, argCount);
        }

        //[DllImport(DLL_x86, EntryPoint = "jsCallGlobal", CallingConvention = CallingConvention.Cdecl)]
        //public static extern long jsCallGlobal(IntPtr es, long func, [MarshalAs(UnmanagedType.LPArray)] long[] args,
        //    int argCount);

        [DllImport(DLL_x86, EntryPoint = "jsGetGlobal", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern long jsGetGlobal_x86(IntPtr es, string prop);

        [DllImport(DLL_x64, EntryPoint = "jsGetGlobal", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern long jsGetGlobal_x64(IntPtr es, string prop);

        public static long jsGetGlobal(IntPtr es, string prop)
        {
            return is64() ? jsGetGlobal_x64(es, prop) : jsGetGlobal_x86(es, prop);
        }

        [DllImport(DLL_x86, EntryPoint = "jsSetGlobal", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void jsSetGlobal_x86(IntPtr es, string prop, long jsValue);

        [DllImport(DLL_x64, EntryPoint = "jsSetGlobal", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern void jsSetGlobal_x64(IntPtr es, string prop, long jsValue);

        public static void jsSetGlobal(IntPtr es, string prop, long jsValue)
        {
            if (is64())
            {
                jsSetGlobal_x64(es, prop, jsValue);
            }
            else
            {
                jsSetGlobal_x86(es, prop, jsValue);
            }
        }

        //[DllImport(DLL_x86, EntryPoint = "jsGC", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void jsGC();

        [DllImport(DLL_x86, EntryPoint = "wkeShowDevtools", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeShowDevtools_x86(IntPtr webView, string path, wkeOnShowDevtoolsCallback callback,
            IntPtr param);
        [DllImport(DLL_x64, EntryPoint = "wkeShowDevtools", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeShowDevtools_x64(IntPtr webView, string path, wkeOnShowDevtoolsCallback callback,
            IntPtr param);
        
        public static void wkeShowDevtools(IntPtr webView, string path, wkeOnShowDevtoolsCallback callback,
            IntPtr param)
        {
            if (is64())
            {
                wkeShowDevtools_x64(webView, path, callback, param);
            }
            else
            {
                wkeShowDevtools_x86(webView, path, callback, param);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetGetHTTPHeaderFieldFromResponse",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern IntPtr wkeNetGetHTTPHeaderFieldFromResponse_x86(IntPtr job, string key);

        [DllImport(DLL_x64, EntryPoint = "wkeNetGetHTTPHeaderFieldFromResponse",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern IntPtr wkeNetGetHTTPHeaderFieldFromResponse_x64(IntPtr job, string key);

        public static IntPtr wkeNetGetHTTPHeaderFieldFromResponse(IntPtr job, string key)
        {
            return is64()
                ? wkeNetGetHTTPHeaderFieldFromResponse_x64(job, key)
                : wkeNetGetHTTPHeaderFieldFromResponse_x86(job, key);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetGetUrlByJob", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern IntPtr wkeNetGetUrlByJob_x86(IntPtr job);

        [DllImport(DLL_x64, EntryPoint = "wkeNetGetUrlByJob", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern IntPtr wkeNetGetUrlByJob_x64(IntPtr job);

        public static IntPtr wkeNetGetUrlByJob(IntPtr job)
        {
            return is64() ? wkeNetGetUrlByJob_x64(job) : wkeNetGetUrlByJob_x86(job);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetGetMIMEType", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern IntPtr wkeNetGetMIMEType_x86(IntPtr job, IntPtr mime);

        [DllImport(DLL_x64, EntryPoint = "wkeNetGetMIMEType", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        private static extern IntPtr wkeNetGetMIMEType_x64(IntPtr job, IntPtr mime);

        public static IntPtr wkeNetGetMIMEType(IntPtr job)
        {
            return is64() ? wkeNetGetMIMEType_x64(job, IntPtr.Zero) : wkeNetGetMIMEType_x86(job, IntPtr.Zero);
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetSetHTTPHeaderField", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeNetSetHTTPHeaderField_x86(IntPtr job, string key, string value, [MarshalAs(UnmanagedType.I1)]bool response);

        [DllImport(DLL_x64, EntryPoint = "wkeNetSetHTTPHeaderField", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeNetSetHTTPHeaderField_x64(IntPtr job, string key, string value, [MarshalAs(UnmanagedType.I1)]bool response);

        public static void wkeNetSetHTTPHeaderField(IntPtr job, string key, string value)
        {
            if (is64())
            {
                wkeNetSetHTTPHeaderField_x64(job, key, value, false);
            }
            else
            {
                wkeNetSetHTTPHeaderField_x86(job, key, value, false);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeNetCancelRequest", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetCancelRequest_x86(IntPtr job);
        [DllImport(DLL_x64, EntryPoint = "wkeNetCancelRequest", CallingConvention = CallingConvention.Cdecl)]
        private static extern void wkeNetCancelRequest_x64(IntPtr job);

        public static void wkeNetCancelRequest(IntPtr job)
        {
            if (is64())
            {
                wkeNetCancelRequest_x64(job);
            }
            else
            {
                wkeNetCancelRequest_x86(job);
            }
        }

        [DllImport(DLL_x86, EntryPoint = "wkeInsertCSSByFrame", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeInsertCSSByFrame_x86(IntPtr webView, IntPtr frameId, string cssText);
        [DllImport(DLL_x64, EntryPoint = "wkeInsertCSSByFrame", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        private static extern void wkeInsertCSSByFrame_x64(IntPtr webView, IntPtr frameId, string cssText);

        public static void wkeInsertCSSByFrame(IntPtr webView, IntPtr frameId, string cssText)
        {
            if (is64())
            {
                wkeInsertCSSByFrame_x64(webView, frameId, cssText);
            }
            else
            {
                wkeInsertCSSByFrame_x86(webView, frameId, cssText);
            }
        }

        public static string Utf8IntptrToString(IntPtr ptr)
        {
            var data = new System.Collections.Generic.List<byte>();
            var off = 0;
            while (true)
            {
                var ch = Marshal.ReadByte(ptr, off++);
                if (ch == 0)
                {
                    break;
                }
                data.Add(ch);
            }
            return System.Text.Encoding.UTF8.GetString(data.ToArray());
        }
    }
}