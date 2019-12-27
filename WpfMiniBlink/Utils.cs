using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Miniblink
{
    internal static class Utils
    {
        public static int LOWORD(IntPtr dword)
        {
            return (int) dword & 65535;
        }

        public static int HIWORD(IntPtr dword)
        {
            return (int) dword >> 16;
        }

        public static IntPtr Dword(IntPtr dword)
        {
            return new IntPtr((IntPtr.Size == 8) ? (int) (dword.ToInt64() << 32 >> 32) : dword.ToInt32());
        }

        public static string[] PtrToStringArray(IntPtr ptr, int length)
        {
            var data = new string[length];

            for (var i = 0; i < length; ++i)
            {
                var str = (IntPtr) Marshal.PtrToStructure(ptr, typeof(IntPtr));
                data[i] = Marshal.PtrToStringAnsi(str);
                ptr = new IntPtr(ptr.ToInt64() + IntPtr.Size);
            }

            return data;
        }

        public static wkePostBodyElement[] PtrToPostElArray(IntPtr ptr, int length)
        {
            var data = new wkePostBodyElement[length];

            for (var i = 0; i < length; ++i)
            {
                var tmp = (IntPtr) Marshal.PtrToStructure(ptr, typeof(IntPtr));
                data[i] = (wkePostBodyElement) Marshal.PtrToStructure(tmp, typeof(wkePostBodyElement));
                ptr = new IntPtr(ptr.ToInt64() + IntPtr.Size);
            }

            return data;
        }

        public static bool IsDesignMode()
        {
            var returnFlag = false;

#if DEBUG
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                returnFlag = true;
            }
            else if (Process.GetCurrentProcess().ProcessName == "devenv")
            {
                returnFlag = true;
            }
#endif

            return returnFlag;
        }

        public static List<Cookie> ParseCookies(string cookies, string defaultDomain = null)
        {
            var regx = new Regex("expires=(.+?GMT)", RegexOptions.IgnoreCase);
            var ms = regx.Match(cookies);
            while (ms.Success)
            {
                var left = cookies.Substring(0, ms.Index);
                var right = cookies.Substring(ms.Index + ms.Length);
                var time = ms.Groups[1].Value;
                DateTime out_time;
                if (DateTime.TryParse(time, out out_time))
                {
                    cookies = left + "expires=" + out_time.ToString("yyyy-MM-dd HH:mm:ss") + right;
                }
                else
                {
                    cookies = left + right;
                }

                ms = regx.Match(cookies);
            }

            var list = new List<Cookie>();
            var items = cookies.Split(';', ',');
            var curr = new Cookie
            {
                Domain = defaultDomain
            };
            for (var i = 0; i < items.Length; i++)
            {
                if (items[i] == null || string.IsNullOrEmpty(items[i].Trim())) continue;

                var item = items[i];
                string value = null;

                switch (item.ToLower().Trim())
                {
                    case "httponly":
                        curr.HttpOnly = true;
                        break;
                    case "secure":
                        curr.Secure = true;
                        break;
                    default:
                        value = item;
                        break;
                }

                if (value == null) continue;

                var kv = value.Split('=');
                var k = kv[0].Trim();
                var v = kv[1].Trim();

                switch (k.ToLower())
                {
                    case "comment":
                        curr.Comment = v;
                        break;
                    case "domain":
                        curr.Domain = v;
                        break;
                    case "max-age":
                        var sec = 0;
                        if (int.TryParse(v, out sec))
                        {
                            curr.Expires = DateTime.Now.AddSeconds(sec);
                        }

                        break;
                    case "expires":
                        curr.Expires = DateTime.Parse(v);
                        break;
                    case "path":
                        curr.Path = v;
                        break;
                    case "version":
                        var ver = 0;
                        if (int.TryParse(v, out ver))
                        {
                            curr.Version = ver;
                        }

                        break;
                    default:
                        curr = new Cookie(k, v)
                        {
                            Domain = defaultDomain
                        };
                        list.Add(curr);
                        break;
                }
            }

            return list;
        }

        public static Image GetReducedImage(int width, int height, Image image)
        {
            var imageFromWidth = image.Width;
            var imageFromHeight = image.Height;
            var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);
            }

            return bmp;
        }

        public static bool IsExtendedKey(Key key)
        {
            switch (key)
            {
                case Key.Insert:
                case Key.Delete:
                case Key.Home:
                case Key.End:
                case Key.Prior:
                case Key.Next:
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    return true;
                default:
                    return false;
            }
        }
    }
}