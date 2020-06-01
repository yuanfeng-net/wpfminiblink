using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miniblink
{
    public class DeviceParameter
    {
        private IMiniblink _mb;

        internal DeviceParameter(IMiniblink miniblink)
        {
            _mb = miniblink;
        }

        /// <summary>
        /// navigator.maxTouchPoints
        /// </summary>
        public int NavigatorMaxTouchPoints
        {
            get { return Convert.ToInt32(_mb.RunJs("return navigator.maxTouchPoints;")); }
            set { MBApi.wkeSetDeviceParameter(_mb.MiniblinkHandle, "navigator.maxTouchPoints", null, value, 0); }
        }

        /// <summary>
        /// navigator.platform
        /// </summary>
        public string NavigatorPlatform
        {
            get { return Convert.ToString(_mb.RunJs("return navigator.platform;")); }
            set { MBApi.wkeSetDeviceParameter(_mb.MiniblinkHandle, "navigator.platform", value, 0, 0); }
        }

        /// <summary>
        /// navigator.hardwareConcurrency
        /// </summary>
        public int HardwareConcurrency
        {
            get { return Convert.ToInt32(_mb.RunJs("return navigator.hardwareConcurrency;")); }
            set
            {
                MBApi.wkeSetDeviceParameter(_mb.MiniblinkHandle, "navigator.hardwareConcurrency", null, value, 0);
            }
        }

        /// <summary>
        /// screen.width
        /// </summary>
        public int ScreenWidth
        {
            get { return Convert.ToInt32(_mb.RunJs("return screen.width;")); }
            set
            {
                MBApi.wkeSetDeviceParameter(_mb.MiniblinkHandle, "screen.width", null, value, 0);
            }
        }

        /// <summary>
        /// screen.height
        /// </summary>
        public int ScreenHeight
        {
            get { return Convert.ToInt32(_mb.RunJs("return screen.height;")); }
            set
            {
                MBApi.wkeSetDeviceParameter(_mb.MiniblinkHandle, "screen.height", null, value, 0);
            }
        }

        /// <summary>
        /// screen.availWidth
        /// </summary>
        public int ScreenAvailWidth
        {
            get { return Convert.ToInt32(_mb.RunJs("return screen.availWidth;")); }
            set
            {
                MBApi.wkeSetDeviceParameter(_mb.MiniblinkHandle, "screen.availWidth", null, value, 0);
            }
        }

        /// <summary>
        /// screen.availHeight
        /// </summary>
        public int ScreenAvailHeight
        {
            get { return Convert.ToInt32(_mb.RunJs("return screen.availHeight;")); }
            set
            {
                MBApi.wkeSetDeviceParameter(_mb.MiniblinkHandle, "screen.availHeight", null, value, 0);
            }
        }

        /// <summary>
        /// screen.pixelDepth
        /// </summary>
        public int ScreenPixelDepth
        {
            get { return Convert.ToInt32(_mb.RunJs("return screen.pixelDepth;")); }
            set
            {
                MBApi.wkeSetDeviceParameter(_mb.MiniblinkHandle, "screen.pixelDepth", null, value, 0);
            }
        }

        /// <summary>
        /// window.devicePixelRatio
        /// </summary>
        public int DevicePixelRatio
        {
            get { return Convert.ToInt32(_mb.RunJs("return window.devicePixelRatio;")); }
            set
            {
                _mb.RunJs("Object.defineProperty(window,'devicePixelRatio',{get:function(){return " + value + ";}})");
            }
        }
    }
}
