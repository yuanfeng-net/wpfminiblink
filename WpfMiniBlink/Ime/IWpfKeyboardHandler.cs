using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace WpfMiniBlink.Ime
{
    public interface IWpfKeyboardHandler : IDisposable
    {
        void Setup(HwndSource source);
        void HandleKeyPress(KeyEventArgs e);
        void HandleTextInput(TextCompositionEventArgs e);
    }
}
