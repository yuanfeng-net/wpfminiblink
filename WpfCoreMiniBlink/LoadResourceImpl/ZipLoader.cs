using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ionic.Zip;

namespace Miniblink.LoadResourceImpl
{
    public class ZipLoader : ILoadResource
    {
        public string Domain { get; }
        private ZipFile _zip;
        private string _pwd;

        static ZipLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("DotNetZip,"))
            {
                var resource = typeof(IMiniblink).Namespace + ".Files.DotNetZip.dll";
                var curAsm = Assembly.GetExecutingAssembly();
                using (var sm = curAsm.GetManifestResourceStream(resource))
                {
                    if (sm == null)
                    {
                        throw new Exception("没有找到DotNetZip.dll");
                    }
                    var data = new byte[sm.Length];
                    sm.Read(data, 0, data.Length);
                    return Assembly.Load(data);
                }
            }

            return null;
        }

        public ZipLoader(string zipPath, string domain, string pwd = null)
        {
            if (File.Exists(zipPath) == false)
            {
                throw new FileNotFoundException(zipPath);
            }
            if (ZipFile.IsZipFile(zipPath) == false)
            {
                throw new NotSupportedException("无法识别的ZIP文件");
            }

            if (pwd != null && ZipFile.CheckZipPassword(zipPath, pwd) == false)
            {
                throw new BadPasswordException("密码不正确");
            }
            _zip = ZipFile.Read(zipPath);
            _zip.Password = pwd;
            _pwd = pwd;
            Domain = domain;
        }

        public ZipLoader(Assembly assembly, string zipPath, string domain, string pwd = null)
        {
            MemoryStream zipsm;
            zipPath = zipPath.TrimStart('/').Replace("/", ".");

            using (var sm = assembly.GetManifestResourceStream(zipPath))
            {
                if (sm == null)
                {
                    throw new Exception("没有找到内嵌资源");
                }
                var data = new byte[sm.Length];
                sm.Read(data, 0, data.Length);
                zipsm = new MemoryStream(data);
            }

            if (ZipFile.IsZipFile(zipsm, false) == false)
            {
                throw new NotSupportedException("无法识别的ZIP文件");
            }

            zipsm.Position = 0;
            _zip = ZipFile.Read(zipsm);
            _zip.Password = pwd;
            _pwd = pwd;
            Domain = domain;
        }


        public byte[] ByUri(Uri uri)
        {
            var path = uri.AbsolutePath.TrimStart('/');
            if (_zip.ContainsEntry(path) == false)
            {
                return null;
            }

            var file = _zip.SelectEntries(path).FirstOrDefault();
            if (file == null)
            {
                return null;
            }

            using (var sm = new MemoryStream())
            {
                if (_pwd != null)
                {
                    file.ExtractWithPassword(sm, _pwd);
                }
                else
                {
                    file.Extract(sm);
                }

                return sm.ToArray();
            }
        }
    }
}
