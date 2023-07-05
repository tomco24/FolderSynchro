using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchro
{
    internal class FolderManager
    {
        public string Path { get; }
        private Dictionary<string, DateTime> _fileList = new Dictionary<string, DateTime>();
        public FolderManager(string path)
        {
            // TODO check path
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }
            Path = path;
        }
        public void InitFileList() {
            _fileList = Directory.GetFiles(Path, "*", SearchOption.AllDirectories).
                ToDictionary(x => x, x => File.GetLastWriteTime(x));
            
        }


    }
}
