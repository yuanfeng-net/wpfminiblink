using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Miniblink
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WinRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public WinRect(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WinSize
    {
        public int width;
        public int height;

        public WinSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WinPaint
    {
        public int hdc;
        public int fErase;
        public WinRect rcPaint;
        public int fRestore;
        public int fIncUpdate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WinPoint
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CompositionForm
    {
        public int dwStyle;
        public WinPoint ptCurrentPos;
        public WinRect rcArea;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WinBitmap
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public short bmPlanes;
        public short bmBitsPixel;
        public int bmBits;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    internal struct BlendFunction
    {
        [FieldOffset(0)]
        public byte BlendOp;
        [FieldOffset(1)]
        public byte BlendFlags;
        [FieldOffset(2)]
        public byte SourceConstantAlpha;
        [FieldOffset(3)]
        public byte AlphaFormat;
    }

	internal struct MARGINS
	{
		public int left;
		public int right;
		public int top;
		public int bottom;
	}
}
