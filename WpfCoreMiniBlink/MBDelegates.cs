using System;
using System.Runtime.InteropServices;

namespace Miniblink
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeTitleChangedCallback(IntPtr webView, IntPtr param, IntPtr title);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeURLChangedCallback(IntPtr webView, IntPtr param, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeURLChangedCallback2(IntPtr webView, IntPtr param, IntPtr frame, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, 
        int x, int y, int cx, int cy);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeAlertBoxCallback(IntPtr webView, IntPtr param, IntPtr msg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate byte wkeConfirmBoxCallback(IntPtr webView, IntPtr param, IntPtr msg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate byte wkePromptBoxCallback(IntPtr webView, IntPtr param, IntPtr msg, IntPtr defaultResult,
        IntPtr result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate byte wkeNavigationCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType,
        IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr wkeCreateViewCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType,
        IntPtr url, IntPtr windowFeatures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeDocumentReadyCallback(IntPtr webView, IntPtr param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeDocumentReady2Callback(IntPtr webView, IntPtr param, IntPtr frame);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeLoadingFinishCallback(IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result,
        IntPtr failedReason);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate byte wkeDownloadCallback(IntPtr webView, IntPtr param, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeConsoleCallback(IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message,
        IntPtr sourceName, uint sourceLine, IntPtr stackTrace);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool wkeLoadUrlBeginCallback(IntPtr webView, IntPtr param, IntPtr url, IntPtr job);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeLoadUrlEndCallback(IntPtr webView, IntPtr param, IntPtr url, IntPtr job, IntPtr buf,
        int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeDidCreateScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frame, IntPtr context,
        int extensionGroup, int worldId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void wkeWillReleaseScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frame,
        IntPtr context, int worldId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long wkeJsNativeFunction(IntPtr jsExecState, IntPtr param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool wkeNetResponseCallback(IntPtr webView, IntPtr param, string url, IntPtr job);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate long jsGetPropertyCallback(IntPtr es, long obj, string propertyName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate bool jsSetPropertyCallback(IntPtr es, long obj, string propertyName, long value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long jsCallAsFunctionCallback(IntPtr es, long obj, IntPtr args, int argCount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void jsFinalizeCallback(IntPtr data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void wkeOnShowDevtoolsCallback(IntPtr webView, IntPtr param);
}
