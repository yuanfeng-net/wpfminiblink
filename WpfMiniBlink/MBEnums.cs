using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniblink
{
    internal enum wkeMouseFlags
    {
        WKE_LBUTTON = 0x01,
        WKE_RBUTTON = 0x02,
        WKE_SHIFT = 0x04,
        WKE_CONTROL = 0x08,
        WKE_MBUTTON = 0x10,
    }

    internal enum wkeKeyFlags
    {
        WKE_EXTENDED = 0x0100,
        WKE_REPEAT = 0x4000,
    }

    public enum wkeRequestType
    {
        Invalidation,
        Get,
        Post,
        Put
    }

    public enum jsType
    {
        NUMBER = 0,
        STRING = 1,
        BOOLEAN = 2,
        OBJECT = 3,
        FUNCTION = 4,
        UNDEFINED = 5,
        ARRAY = 6,
        NULL = 7
    }

    /// <summary>
    /// 控制台消息等级
    /// </summary>
    public enum wkeConsoleLevel
    {
        Debug = 4,
        Log = 1,
        Info = 5,
        Warning = 2,
        Error = 3,
        RevokedError = 6,
    }
    /// <summary>
    /// 载入返回值
    /// </summary>
    public enum wkeLoadingResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeeded,
        /// <summary>
        /// 失败
        /// </summary>
        Failed,
        /// <summary>
        /// 取消
        /// </summary>
        Canceled
    }
    /// <summary>
    /// 导航类型
    /// </summary>
    public enum wkeNavigationType
    {
        /// <summary>
        /// 单击链接
        /// </summary>
        LinkClick,
        /// <summary>
        /// 表单提交submit
        /// </summary>
        FormSubmit,
        /// <summary>
        /// 前进或后退
        /// </summary>
        BackForward,
        /// <summary>
        /// 重新载入
        /// </summary>
        ReLoad,
        /// <summary>
        /// 表单重新提交resubmit
        /// </summary>
        FormReSubmit,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }

    public enum wkeCursorInfo
    {
        Pointer,
        Cross,
        Hand,
        IBeam,
        Wait,
        Help,
        EastResize,
        NorthResize,
        NorthEastResize,
        NorthWestResize,
        SouthResize,
        SouthEastResize,
        SouthWestResize,
        WestResize,
        NorthSouthResize,
        EastWestResize,
        NorthEastSouthWestResize,
        NorthWestSouthEastResize,
        ColumnResize,
        RowResize,
        MiddlePanning,
        EastPanning,
        NorthPanning,
        NorthEastPanning,
        NorthWestPanning,
        SouthPanning,
        SouthEastPanning,
        SouthWestPanning,
        WestPanning,
        Move,
        VerticalText,
        Cell,
        ContextMenu,
        Alias,
        Progress,
        NoDrop,
        Copy,
        None,
        NotAllowed,
        ZoomIn,
        ZoomOut,
        Grab,
        Grabbing,
        Custom
    }

    /// <summary>
    /// Cookie命令
    /// </summary>
    public enum wkeCookieCommand
    {
        /// <summary>
        /// 清空所有Cookies
        /// </summary>
        ClearAllCookies,
        /// <summary>
        /// 清空会话Cookies
        /// </summary>
        ClearSessionCookies,
        /// <summary>
        /// 将Cookies刷新到文件
        /// </summary>
        FlushCookiesToFile,
        /// <summary>
        /// 从文件重新载入Cookies
        /// </summary>
        ReloadCookiesFromFile
    }

    /// <summary>
    /// 代理类型
    /// </summary>
    public enum wkeProxyType
    {
        NONE,
        HTTP,
        SOCKS4,
        SOCKS4A,
        SOCKS5,
        SOCKS5HOSTNAME
    }
    /// <summary>
    /// 设置的掩码
    /// </summary>
    public enum wkeSettingMask
    {
        /// <summary>
        /// 代理有效
        /// </summary>
        PROXY = 1,
        /// <summary>
        /// 重画回调在其他线程
        /// </summary>
        PAINTCALLBACK_IN_OTHER_THREAD = 4,
    }

    public enum wkeHttBodyElementType
    {
        wkeHttBodyElementTypeData,
        wkeHttBodyElementTypeFile
    }
}
