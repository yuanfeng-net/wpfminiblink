using System;
using System.Runtime.InteropServices;

namespace Miniblink
{
    internal class WinApi
    {
        [DllImport("user32.dll", EntryPoint = "GetWindowLongW", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong86(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong64(IntPtr hwnd, int nIndex);

        public static int GetWindowLong(IntPtr hwnd, int nIndex)
        {
            if (IntPtr.Size == 8)
            {
                return GetWindowLong64(hwnd, nIndex);
            }
            return GetWindowLong86(hwnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongW", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowLong86(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowLong64(IntPtr hwnd, int nIndex, int dwNewLong);

        public static IntPtr SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLong64(hwnd, nIndex, dwNewLong);
            }
            return SetWindowLong86(hwnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowLong64(IntPtr hwnd, int nIndex, Delegate dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongW", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowLong86(IntPtr hwnd, int nIndex, Delegate dwNewLong);

        public static IntPtr SetWindowLong(IntPtr hwnd, int nIndex, Delegate dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLong64(hwnd, nIndex, dwNewLong);
            }
            return SetWindowLong86(hwnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "CallWindowProcW")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, int msg, IntPtr wParam,
            IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetClientRect")]
        public static extern int GetClientRect(IntPtr hwnd, ref WinRect lpRect);

        [DllImport("user32.dll", EntryPoint = "BeginPaint")]
        public static extern IntPtr BeginPaint(IntPtr hwnd, ref WinPaint lpPaint);

        [DllImport("user32.dll", EntryPoint = "IntersectRect")]
        public static extern int IntersectRect(ref WinRect lpDestRect, ref WinRect lpSrc1Rect, ref WinRect lpSrc2Rect);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern int BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc,
            int ySrc, int dwRop);

        [DllImport("user32.dll", EntryPoint = "EndPaint")]
        public static extern int EndPaint(IntPtr hwnd, ref WinPaint lpPaint);

        [DllImport("user32.dll", EntryPoint = "GetFocus")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", EntryPoint = "SetFocus")]
        public static extern IntPtr SetFocus(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "SetCapture")]
        public static extern int SetCapture(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern int ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "ScreenToClient")]
        public static extern int ScreenToClient(IntPtr hwnd, ref WinPoint lpPoint);

        [DllImport("imm32.dll", EntryPoint = "ImmGetContext")]
        public static extern IntPtr ImmGetContext(IntPtr hwnd);

        [DllImport("imm32.dll", EntryPoint = "ImmSetCompositionWindow")]
        public static extern int ImmSetCompositionWindow(IntPtr himc, ref CompositionForm lpCompositionForm);

        [DllImport("imm32.dll", EntryPoint = "ImmReleaseContext")]
        public static extern int ImmReleaseContext(IntPtr hwnd, IntPtr himc);

        [DllImport("user32.dll", EntryPoint = "DefWindowProcA")]
        public static extern IntPtr DefWindowProc(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        public static extern int GetWindowRect(IntPtr hwnd, ref WinRect lpRect);

        [DllImport("user32.dll", EntryPoint = "OffsetRect")]
        public static extern int OffsetRect(ref WinRect lpRect, int x, int y);

        [DllImport("gdi32.dll", EntryPoint = "GetCurrentObject")]
        public static extern IntPtr GetCurrentObject(IntPtr hdc, int uObjectType);

        [DllImport("gdi32.dll", EntryPoint = "GetObjectW")]
        public static extern int GetObject(IntPtr hObject, int nCount, ref WinBitmap lpObject);

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "UpdateLayeredWindow")]
        public static extern int UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, IntPtr pptDst, ref WinSize psize,
            IntPtr hdcSrc, ref WinPoint pptSrc, int crKey, ref BlendFunction pblend, int dwFlags);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern int DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern int DeleteDC(IntPtr hdc);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32.dll", EntryPoint = "InvalidateRect")]
        public static extern int InvalidateRect(IntPtr hwnd, ref WinRect lpRect, bool bErase);

        [DllImport("kernel32.dll", EntryPoint = "lstrlenA")]
        public static extern int lstrlen(IntPtr lpString);

	    [DllImport("dwmapi.dll")]
	    public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

	    [DllImport("dwmapi.dll")]
	    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

	    [DllImport("dwmapi.dll")]
	    public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
	}
}
