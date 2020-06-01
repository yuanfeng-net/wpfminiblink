using System;
using System.IO;

namespace Miniblink.LoadResourceImpl
{
    public class FileLoader : ILoadResource
    {
        private string _domain;
        private string _dir;

        public FileLoader(string dir, string domain)
        {
            _dir = dir.TrimEnd('/');
            _domain = domain.TrimEnd('/');
        }

        public byte[] ByUri(Uri uri)
        {
            var path = _dir + uri.AbsolutePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }

        public string Domain => _domain;
    }
}
