using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Miniblink.LocalHttp
{
    public class NetApiEngine
    {
        public string Domain { get; }
        public event Func<Assembly, bool> LoadingAssembly;
        public event Func<Type, bool> LoadingType;
        public event EventHandler<NetApiBefore> ExecuteBefore;
        public event EventHandler<NetApiAfter> ExecuteAfter;
        public event EventHandler<NetApiError> ExecuteError;

        private HttpListener _http;

        private Dictionary<string, RouteSetting> _getSettings
            = new Dictionary<string, RouteSetting>(StringComparer.OrdinalIgnoreCase);

        private Dictionary<string, RouteSetting> _postSettings
            = new Dictionary<string, RouteSetting>(StringComparer.OrdinalIgnoreCase);

        public NetApiEngine(int? port = null)
        {
            ScanAssemblies();

            _http = new HttpListener();

            port = port ?? 23000;

            while (PortInUse(port.Value))
            {
                port++;
            }
            Domain = "http://localhost:" + port;

            _http.Prefixes.Add(Domain + "/");
            _http.Start();
            Task.Factory.StartNew(ListenerContext);
        }

        private void ListenerContext()
        {
            var waiter = new AutoResetEvent(false);

            while (_http.IsListening)
            {
                waiter.Reset();

                _http.BeginGetContext(Process, waiter);

                waiter.WaitOne();
            }
        }

        private void Process(IAsyncResult ar)
        {
            var waiter = (AutoResetEvent)ar.AsyncState;
            var context = _http.EndGetContext(ar);
            waiter.Set();
            var request = new NetApiRequest(context.Request);
            var setting = GetSetting(request.RequestMethod, request.Url.AbsolutePath);
            if (setting == null) return;

            var before = new NetApiBefore(request)
            {
                Instance = TypeResolve(setting.ClassType),
                Method = setting.Method
            };

            try
            {
                ExecuteBefore?.Invoke(this, before);

                if (before.Result == null)
                {
                    ((NetApi)before.Instance).Request = request;
                    before.Result = before.Method.Invoke(before.Instance, null)?.ToString();
                }

                ExecuteAfter?.Invoke(this, new NetApiAfter(request)
                {
                    Instance = before.Instance,
                    Method = before.Method,
                    Result = before.Result
                });
            }
            catch (Exception ex)
            {
                var err = new NetApiError(request)
                {
                    Error = ex,
                    Instance = before.Instance,
                    Method = before.Method
                };
                ExecuteError?.Invoke(this, err);
                if (err.Result != null)
                    before.Result = err.Result;
            }

            var result = before.Result ?? string.Empty;
            var data = Encoding.UTF8.GetBytes(result);
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = data.Length;
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.StatusCode = 200;
            context.Response.OutputStream.Write(data, 0, data.Length);
            context.Response.Close();
        }

        protected virtual object TypeResolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        protected RouteSetting GetSetting(RequestType type, string path)
        {
            if (type == RequestType.GET && _getSettings.ContainsKey(path))
            {
                return _getSettings[path];
            }
            if (type == RequestType.POST && _postSettings.ContainsKey(path))
            {
                return _postSettings[path];
            }
            return null;
        }

        protected virtual IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        protected virtual string OnPath(string path)
        {
            return path;
        }

        private void ScanAssemblies()
        {
            var assemblies = GetAssemblies();

            assemblies = assemblies.Where(i =>
            {
                var name = i.GetName().Name;
                if (name.SW("QQ2564874169") ||
                    name.SW("system"))
                    return false;

                return LoadingAssembly == null || LoadingAssembly(i);

            }).ToArray();

            var types = assemblies.SelectMany(i => i.GetTypes()).Where(t =>
            {
                if (t.IsAbstract || !t.IsClass || t.IsGenericType)
                    return false;

                if (t.IsSubclassOf(typeof(NetApi)) == false)
                    return false;

                return LoadingType == null || LoadingType(t);

            }).ToArray();

            foreach (var t in types)
            {
                var ms = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                foreach (var m in ms)
                {
                    var setting = m.GetCustomAttribute<RouteAttribute>();
                    if (setting == null)
                        continue;

                    var apiname = t.GetCustomAttribute<ApiNameAttribute>()?.Name ?? t.Name;
                    var path = setting.Path;
                    if (path == null)
                    {
                        path = "/" + apiname + "/" + m.Name;
                    }
                    else if (path.SW("/") == false)
                    {
                        path = "/" + apiname + "/" + path;
                    }
                    var rs = new RouteSetting
                    {
                        ClassType = t,
                        Method = m,
                        Type = setting.Type,
                        Path = OnPath(path)
                    };
                    if (rs.Type == RequestType.GET)
                    {
                        _getSettings[rs.Path] = rs;
                    }
                    else if (rs.Type == RequestType.POST)
                    {
                        _postSettings[rs.Path] = rs;
                    }
                }
            }
        }

        private static bool PortInUse(int port)
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var ipEndPoints = ipProperties.GetActiveTcpListeners();
            foreach (var endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
