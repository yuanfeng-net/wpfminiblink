using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Miniblink.LocalHttp
{
    public class NetApiRequest
    {
        public Uri Url => _request.Url;

        public RequestType RequestMethod
        {
            get
            {
                if (_request.HttpMethod.SW("get"))
                    return RequestType.GET;
                if (_request.HttpMethod.SW("post"))
                    return RequestType.POST;
                return RequestType.Unkonw;
            }
        }

        private HttpListenerRequest _request;
        private Dictionary<string, string> _form = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> _get = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        internal NetApiRequest(HttpListenerRequest request)
        {
            _request = request;

            ParseGet();
            ParseForm();
        }

        private void ParseGet()
        {
            var index = _request.RawUrl.IndexOf("?", StringComparison.Ordinal);
            if (index < 0) return;
            var url = _request.RawUrl.Substring(index + 1);
            url = Uri.UnescapeDataString(url).Replace("+", " ");
            var kvs = url.Split('&');
            foreach (var item in kvs)
            {
                index = item.IndexOf("=", StringComparison.Ordinal) + 1;
                if (index < 1) continue;
                var k = item.Substring(0, index - 1);
                var v = item.Length > index ? item.Substring(index) : string.Empty;
                _get[k] = v;
            }
        }

        private void ParseForm()
        {
            if (_request.HasEntityBody == false)
                return;

            var data = new byte[_request.ContentLength64];
            _request.InputStream.Read(data, 0, data.Length);

            var body = Uri.UnescapeDataString(Encoding.UTF8.GetString(data)).Replace("+", " ");
            var kvs = body.Split('&');
            foreach (var item in kvs)
            {
                var index = item.IndexOf("=", StringComparison.Ordinal) + 1;
                if (index < 1) continue;
                var k = item.Substring(0, index - 1);
                var v = item.Length > index ? item.Substring(index) : string.Empty;
                _form[k] = v;
            }
        }

        public string Form(string name)
        {
            return _form.ContainsKey(name) ? _form[name] : null;
        }

        public string Query(string name)
        {
            return _get.ContainsKey(name) ? _get[name] : null;
        }

        public string Param(string name)
        {
            return Query(name) ?? Form(name);
        }

        public string Header(string name)
        {
            return _request.Headers.Get(name);
        }
    }
}
