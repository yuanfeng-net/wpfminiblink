using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Miniblink
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct wkeRect
    {
        public int x;
        public int y;
        public int w;
        public int h;
    }

    /// <summary>
    /// 代理
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WKEProxy
    {
        /// <summary>
        /// 代理类型
        /// </summary>
        public wkeProxyType Type;
        /// <summary>
        /// 服务器名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string HostName;
        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port;
        /// <summary>
        /// 用户名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string Password;
    }

    /// <summary>
    /// 设置
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct WKESettings
    {
        /// <summary>
        /// 代理
        /// </summary>
        public WKEProxy Proxy;
        /// <summary>
        /// 设置掩码，掩码包含
        /// </summary>
        public wkeSettingMask Mask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WKEWindowFeatures
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        [MarshalAs(UnmanagedType.I1)]
        public bool MenuBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool StatusBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool ToolBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool LocationBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool ScrollbarsVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool Resizable;
        [MarshalAs(UnmanagedType.I1)]
        public bool Fullscreen;
    }

    internal struct jsData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string typeName;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsGetPropertyCallback propertyGet;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsSetPropertyCallback propertySet;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsFinalizeCallback finalize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsCallAsFunctionCallback callAsFunction;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct jsKeys
    {
        public int length;
        public IntPtr keys;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct wkePostBodyElement
    {
        public int size;
        public wkeHttBodyElementType type;
        public IntPtr data;
        public string filePath;
        public long fileStart;
        public long fileLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct wkePostBodyElements
    {
        public int size;
        public IntPtr elements;
        public int elementSize;
        public bool isDirty;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeSlist
    {
        public IntPtr str;
        public IntPtr next;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct wkeMemBuf
    {
        public int size;
        public IntPtr data;
        public int length;
    }
}
