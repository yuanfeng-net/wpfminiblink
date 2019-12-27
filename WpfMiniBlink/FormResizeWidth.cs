using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miniblink
{
    public class FormResizeWidth
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public void SetAll(int width)
        {
            Left = Top = Right = Bottom = width;
        }

        internal FormResizeWidth(int width)
        {
            SetAll(width);
        }
    }
}
