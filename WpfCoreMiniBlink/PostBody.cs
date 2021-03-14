using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Miniblink
{
    public class PostBody
    {
        private Dictionary<string, string> _param;
        private string rawData;
        internal PostBody(IntPtr job)
        {
            _param = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Load(job);
        }

        private void Load(IntPtr job)
        {
            var ptr = MBApi.wkeNetGetPostBody(job);
            var els = (wkePostBodyElements)Marshal.PtrToStructure(ptr, typeof(wkePostBodyElements));
            var elary = Utils.PtrToPostElArray(els.elements, els.elementSize);
            foreach (var item in elary)
            {
                if (item.type == wkeHttBodyElementType.wkeHttBodyElementTypeData)
                {
                    var mem = (wkeMemBuf)Marshal.PtrToStructure(item.data, typeof(wkeMemBuf));
                    var buf = new byte[mem.length];
                    Marshal.Copy(mem.data, buf, 0, buf.Length);
                    rawData = Encoding.UTF8.GetString(buf);
                    ParseData(rawData);
                }
                else if (item.type == wkeHttBodyElementType.wkeHttBodyElementTypeFile)
                {
                    throw new NotSupportedException("file in post");
                }
            }
        }

        private void ParseData(string data)
        {
            if (data == null || data.StartsWith("--"))
                return;

            var items = data.Split('&');

            foreach (var item in items)
            {
                var vals = item.Split('=');
                if (vals.Length < 1)
                    continue;

                if (vals.Length == 1)
                {
                    _param[vals[0]] = string.Empty;
                }
                else
                {
                    var index = item.IndexOf("=", StringComparison.Ordinal) + 1;
                    _param[vals[0]] = item.Length > index ? item.Substring(index) : string.Empty;
                }
            }
        }

        public string GetRawData()
        {
            return rawData;
        }

        public string GetValue(string name)
        {
            return _param.ContainsKey(name) ? _param[name] : null;
        }

        public ICollection<string> GetNames()
        {
            return _param.Keys;
        }
    }

    public class HttpFile : MemoryStream
    {
        public string FileName { get; internal set; }
    }

   
}
