using Miniblink;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace WpfMiniBlink.Ime
{

    public class WpfImeKeyboardHandler : WpfKeyboardHandler
    {
        private int languageCodeId;
        private bool systemCaret;
        private bool isSetup;
        private List<Rect> compositionBounds = new List<Rect>();
        private HwndSource source;
        private HwndSourceHook sourceHook;
        private bool hasImeComposition;
        private MouseButtonEventHandler mouseDownEventHandler;
        private bool isActive;

        public WpfImeKeyboardHandler(MiniblinkBrowser owner) : base(owner)
        {
        }

        public void ChangeCompositionRange(Range selectionRange, Rect[] characterBounds)
        {
            if (!isActive)
            {
                return;
            }

            //var screenInfo = ((IRenderWebBrowser)owner).GetScreenInfo();
            //var scaleFactor = screenInfo.HasValue ? screenInfo.Value.DeviceScaleFactor : 1.0f;

            //This is called on the CEF UI thread, we need to invoke back onte main UI thread to
            //access the UI controls
            //owner.UiThreadRunAsync(() =>
            //{
            //    //TODO: Getting the root window for every composition range change seems expensive,
            //    //we should cache the position and update it on window move.
            //    var parentWindow = Window.GetWindow(owner);
            //    if (parentWindow != null)
            //    {
            //        //TODO: What are we calculating here exactly???
            //        var point = owner.TransformToAncestor(parentWindow).Transform(new Point(0, 0));

            //        var rects = new List<Rect>();

            //        foreach (var item in characterBounds)
            //        {
            //            rects.Add(new Rect(
            //                (int)((point.X + item.X) * scaleFactor),
            //                (int)((point.Y + item.Y) * scaleFactor),
            //                (int)(item.Width * scaleFactor),
            //                (int)(item.Height * scaleFactor)));
            //        }

            //        compositionBounds = rects;
            //        MoveImeWindow(source.Handle);
            //    }
            //});
        }

        public override void Setup(HwndSource source)
        {
            if (isSetup)
            {
                return;
            }

            isSetup = true;

            this.source = source;
            sourceHook = SourceHook;
            source.AddHook(SourceHook);

            owner.GotFocus += OwnerGotFocus;
            owner.LostFocus += OwnerLostFocus;

            mouseDownEventHandler = new MouseButtonEventHandler(OwnerMouseDown);

            owner.AddHandler(UIElement.MouseDownEvent, mouseDownEventHandler, true);

            // If the owner had focus before adding the handler then we have to run the "got focus" code here
            // or it won't set up IME properly in all cases
            if (owner.IsFocused)
            {
                SetActive();
            }
        }

        public override void Dispose()
        {
            // Note Setup can be run after disposing, to "reset" this instance
            // due to the code in ChromiumWebBrowser.PresentationSourceChangedHandler
            if (!isSetup)
            {
                return;
            }

            isSetup = false;

            owner.GotFocus -= OwnerGotFocus;
            owner.LostFocus -= OwnerLostFocus;

            owner.RemoveHandler(UIElement.MouseDownEvent, mouseDownEventHandler);

            if (source != null && sourceHook != null)
            {
                source.RemoveHook(sourceHook);
                source = null;
            }
        }

        private void OwnerMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseImeComposition();
        }

        private void OwnerGotFocus(object sender, RoutedEventArgs e)
        {
            SetActive();
        }

        private void OwnerLostFocus(object sender, RoutedEventArgs e)
        {
            SetInactive();
        }

        private void SetActive()
        {
            // Set to false first if not already, because the value change (and raising of changes)
            // between false and true is necessary for IME to work in all circumstances
            if (InputMethod.GetIsInputMethodEnabled(owner))
            {
                InputMethod.SetIsInputMethodEnabled(owner, false);
            }
            if (InputMethod.GetIsInputMethodSuspended(owner))
            {
                InputMethod.SetIsInputMethodSuspended(owner, false);
            }

            // These calls are needed in order for IME to function correctly.
            InputMethod.SetIsInputMethodEnabled(owner, true);
            InputMethod.SetIsInputMethodSuspended(owner, true);

            isActive = true;
        }

        private void SetInactive()
        {
            isActive = false;

            // These calls are needed in order for IME to function correctly.
            InputMethod.SetIsInputMethodEnabled(owner, false);
            InputMethod.SetIsInputMethodSuspended(owner, false);
        }

        private IntPtr SourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            if (!isActive || !isSetup || owner == null )
            {
                return IntPtr.Zero;
            }

            switch ((WM)msg)
            {
                case WM.IME_SETCONTEXT:
                    {
                        OnImeSetContext(hwnd, (uint)msg, wParam, lParam);
                        handled = true;
                        break;
                    }
                case WM.IME_STARTCOMPOSITION:
                    {
                        OnIMEStartComposition(hwnd);
                        hasImeComposition = true;
                        handled = true;
                        break;
                    }
                case WM.IME_COMPOSITION:
                    {
                        OnImeComposition(hwnd, lParam.ToInt32());
                        handled = true;
                        break;
                    }
                case WM.IME_ENDCOMPOSITION:
                    {
                        OnImeEndComposition(hwnd);
                        hasImeComposition = false;
                        handled = true;
                        break;
                    }
            }

            return IntPtr.Zero;
        }

        private void CloseImeComposition()
        {
            if (hasImeComposition)
            {
                // Set focus to 0, which destroys IME suggestions window.
                ImeNative.SetFocus(IntPtr.Zero);
                // Restore focus.
                ImeNative.SetFocus(source.Handle);
            }
        }

        private void OnImeComposition(IntPtr hwnd, int lParam)
        {
            string text = string.Empty;

            if (ImeHandler.GetResult(hwnd, (uint)lParam, out text))
            {
                //输入完成
                owner.TextInput(text);
            }
            else
            {
                var underlines = new List<CompositionUnderline>();
                int compositionStart = 0;

                if (ImeHandler.GetComposition(hwnd, (uint)lParam, underlines, ref compositionStart, out text))
                {
                    UpdateCaretPosition(compositionStart - 1);
                }
                else
                {
                    CancelComposition(hwnd);
                }
            }
        }

        public void CancelComposition(IntPtr hwnd)
        {
            //DestroyImeWindow(hwnd);
        }

        private void OnImeEndComposition(IntPtr hwnd)
        {
           
            //DestroyImeWindow(hwnd);
        }

        private void OnImeSetContext(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            CreateImeWindow(hwnd);
            MoveImeWindow(hwnd);
        }

        private void OnIMEStartComposition(IntPtr hwnd)
        {
            CreateImeWindow(hwnd);
            MoveImeWindow(hwnd);
        }

        private void CreateImeWindow(IntPtr hwnd)
        {
            languageCodeId = PrimaryLangId(InputLanguageManager.Current.CurrentInputLanguage.KeyboardLayoutId);

            if (languageCodeId == ImeNative.LANG_JAPANESE || languageCodeId == ImeNative.LANG_CHINESE)
            {
                if (!systemCaret)
                {
                    if (ImeNative.CreateCaret(hwnd, IntPtr.Zero, 1, 1))
                    {
                        systemCaret = true;
                    }
                }
            }
        }

        private int PrimaryLangId(int lgid)
        {
            return lgid & 0x3ff;
        }

        private void MoveImeWindow(IntPtr hwnd)
        {
            var hIMC = ImeNative.ImmGetContext(hwnd);

            var rc = MBApi.wkeGetCaretRect(owner.MiniblinkHandle);

            var x = rc.x + rc.w;
            var y = rc.y + rc.h;

            const int kCaretMargin = 1;

            var candidatePosition = new ImeNative.CANDIDATEFORM
            {
                dwIndex = 0,
                dwStyle = (int)ImeNative.CFS_CANDIDATEPOS,
                ptCurrentPos = new ImeNative.POINT(x, y),
                rcArea = new ImeNative.RECT(0, 0, 0, 0)
            };
            ImeNative.ImmSetCandidateWindow(hIMC, ref candidatePosition);

            if (systemCaret)
            {
                ImeNative.SetCaretPos(x, y);
            }

            if (languageCodeId == ImeNative.LANG_KOREAN)
            {
                y += kCaretMargin;
            }
            var excludeRectangle = new ImeNative.CANDIDATEFORM
            {
                dwIndex = 0,
                dwStyle = (int)ImeNative.CFS_EXCLUDE,
                ptCurrentPos = new ImeNative.POINT(x, y),
                rcArea = new ImeNative.RECT(rc.x, rc.y, x, y + kCaretMargin)
            };
            ImeNative.ImmSetCandidateWindow(hIMC, ref excludeRectangle);

            ImeNative.ImmReleaseContext(hwnd, hIMC);
        }

        //销毁输入法窗口
        private void DestroyImeWindow(IntPtr hwnd)
        {
            if (systemCaret)
            {
                //清除位置信息
                ImeNative.DestroyCaret();
                systemCaret = false;
            }
        }

        //更新位置
        private void UpdateCaretPosition(int index)
        {
            MoveImeWindow(source.Handle);
        }
    }

    /// <summary>
    /// Represents a rectangle
    /// </summary>
    [DebuggerDisplay("X = {X}, Y = {Y}, Width = {Width}, Height = {Height}")]
    public struct Rect
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Rect
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public Rect(int x, int y, int width, int height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Returns a new Rect with Scaled values
        /// </summary>
        /// <param name="dpi">Dpi to scale by</param>
        /// <returns>New rect with scaled values</returns>
        public Rect ScaleByDpi(float dpi)
        {
            var x = (int)Math.Ceiling(X / dpi);
            var y = (int)Math.Ceiling(Y / dpi);
            var width = (int)Math.Ceiling(Width / dpi);
            var height = (int)Math.Ceiling(Height / dpi);

            return new Rect(x, y, width, height);
        }
    }

}
