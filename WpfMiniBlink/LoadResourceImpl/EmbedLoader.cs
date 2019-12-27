using System;
using System.Reflection;

namespace Miniblink.LoadResourceImpl
{
    public class EmbedLoader : ILoadResource
    {
        private Assembly _assembly;
        private string _dir;
        private string _namespace;
        private string _domain;

        /// <summary>
        /// 加载程序资源文件
        /// </summary>
        /// <param name="resAssembly">程序集名称</param>
        /// <param name="resDir">资源文件夹名称</param>
        /// <param name="domain">本地网站域名</param>
        public EmbedLoader(Assembly resAssembly, string resDir, string domain)
        {
            _domain = domain;
            _assembly = resAssembly;
            _dir = resDir;
            _namespace = resAssembly.EntryPoint.DeclaringType?.Namespace;
        }

        public byte[] ByUri(Uri uri)
        {
            var path = string.Join(".", _namespace, _dir, uri.AbsolutePath.TrimStart('/').Replace("/", "."));

            using (var sm = _assembly.GetManifestResourceStream(path))
            {
                if (sm == null)
                {
                    return null;
                }

                var data = new byte[sm.Length];
                sm.Read(data, 0, data.Length);
                return data;
            }
        }

        public string Domain => _domain;
    }
}
