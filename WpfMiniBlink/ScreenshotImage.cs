using System;
using System.Collections.Generic;
using System.Drawing;

namespace Miniblink
{
    public class ScreenshotImage : IDisposable
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private List<Image> _data = new List<Image>();

        public ScreenshotImage(int width, int height, IEnumerable<Image> images)
        {
            Width = width;
            Height = height;
            _data.AddRange(images);
        }

        public Image GetImage(int x, int y, int width, int height)
        {
            var img = new Bitmap(width, height);

            var itemY = 0;
            var imgs = new List<Image>(_data);
            while (imgs.Count > 0)
            {
                var item = imgs[0];
                if (itemY + item.Height > y)
                {
                    itemY = y - itemY;
                    break;
                }
                else
                {
                    itemY += item.Height;
                    imgs.RemoveAt(0);
                }
            }

            if (imgs.Count == 0)
            {
                return img;
            }

            using (var gdi = Graphics.FromImage(img))
            {
                var srcY = 0;
                foreach (var item in imgs)
                {
                    var dest = new Rectangle(0, 0, width, item.Height);
                    if (srcY == 0)
                    {
                        dest.Y = itemY;
                        dest.Height = dest.Height - itemY;
                    }
                    if (srcY + dest.Height > height)
                    {
                        dest.Height = height - srcY;
                    }

                    var src = new Rectangle(0, srcY, width, dest.Height);

                    gdi.DrawImage(item, src, dest, GraphicsUnit.Pixel);

                    srcY += dest.Height;

                    if (srcY == height)
                    {
                        break;
                    }
                }
            }

            return img;
        }

        public Image GetImage()
        {
            return GetImage(0, 0, Width, Height);
        }

        public void Dispose()
        {
            if (_data != null)
            {
                foreach (var item in _data)
                {
                    item.Dispose();
                }
                _data.Clear();
                _data = null;
            }
        }
    }
}
