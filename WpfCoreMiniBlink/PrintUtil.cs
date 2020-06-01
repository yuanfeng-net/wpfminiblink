using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Miniblink
{
    internal class PrintUtil
    {
        private IMiniblink _miniblink;
        private ScreenshotImage _screenshotImage;
        private Action<PrintDialog> _callback;
        private int _preY;

        public PrintUtil(IMiniblink miniblink)
        {
            _miniblink = miniblink;
        }

        public void Start(Action<PrintDialog> callback)
        {
            _callback = callback;
            _miniblink.DrawToBitmap(img =>
            {
                _screenshotImage = img;
                img.GetImage().Save(Guid.NewGuid() + ".png");

                var pre = CreatePrintDialog();
                //pre.Document.BeginPrint += Document_BeginPrint;
                //pre.Document.PrintPage += Document_PrintPage;
                _callback?.Invoke(pre);
            });
        }

        private PrintDialog CreatePrintDialog()
        {
            var pre = new PrintDialog {  };
            //pre.Closed += (s, e) =>
            //{
            //    _screenshotImage.Dispose();
            //    _screenshotImage = null;
            //};
            //ToolStrip strip = null;
            //foreach (var control in pre.Controls)
            //{
            //    if (control is ToolStrip)
            //    {
            //        strip = (ToolStrip) control;
            //        break;
            //    }
            //}

            //if (strip != null)
            //{
            //    var btn1 = new ToolStripButton("打印机设置");
            //    btn1.Click += (s, e) =>
            //    {
            //        var dlg = new PrintDialog();
            //        dlg.Document = pre.Document;
            //        dlg.ShowDialog();
            //    };
            //    var btn2 = new ToolStripButton("页面设置");
            //    btn2.Click += (s, e) =>
            //    {
            //        var dlg = new PageSetupDialog();
            //        dlg.Document = pre.Document;
            //        if (dlg.ShowDialog() == DialogResult.OK)
            //        {
            //            pre.PrintPreviewControl.InvalidatePreview();
            //        }
            //    };
            //    strip.Items.Insert(1, btn1);
            //    strip.Items.Insert(2, btn2);
            //}

            return pre;
        }

        //private void Document_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    var margin = e.MarginBounds;
        //    var w = margin.Width;
        //    var h = margin.Height;
        //    var srcW = _screenshotImage.Width;
        //    var rate = (double)srcW / w;
        //    var srcH = (int)(h * rate);

        //    using (var img = _screenshotImage.GetImage(0, _preY, srcW, srcH))
        //    {
        //        using (var reduce = Utils.GetReducedImage(w, h, img))
        //        {
        //            e.Graphics.DrawImage(reduce,
        //                new Rectangle(margin.X, margin.Y, reduce.Width, reduce.Height),
        //                new Rectangle(0, 0, reduce.Width, reduce.Height),
        //                GraphicsUnit.Pixel);
        //        }
        //    }

        //    e.HasMorePages = _preY + srcH <= _screenshotImage.Height;
        //    _preY += srcH;
        //}

        //private void Document_BeginPrint(object sender, PrintEventArgs e)
        //{
        //    _preY = 0;
        //}
    }
}
