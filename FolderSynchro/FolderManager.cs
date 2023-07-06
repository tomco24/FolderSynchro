using FolderSynchro.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchro
{
    internal class FolderManager
    {
        public string FolderPath { get; }
        public string FolderName { get; }
        internal List<FolderManager> Managers { get => _managers; set => _managers = value; }
        internal FolderManager? Parent { get; }
        private List<FolderManager> _managers = new List<FolderManager>();
        private Dictionary<string, DateTime> _fileList = new Dictionary<string, DateTime>();
        public FolderManager(string path)
        {
            // TODO check path
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }
            FolderPath = path;
            Parent = null;
            FolderName = "";
        }
        public FolderManager(string path, FolderManager parent)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }
            FolderPath = path;
            Parent = parent;
            FolderName = GetFolderName();
        }
        public FolderManager(FolderManager parent, string folderName)
        {
            Parent = parent;
            FolderName = folderName;
            FolderPath = Path.Combine(Parent.FolderPath, folderName);
        }
        
        public List<string> GetFileList() 
        {
            return Directory.GetFiles(FolderPath, "*", SearchOption.TopDirectoryOnly)
                .Select(f => Path.GetFileName(f)).ToList();
        }
        public void InitFileList() {
            _fileList = GetFileList().ToDictionary(x => x, x => File.GetLastWriteTime(x));
            
        }
        public void InitManagers()
        {
            List<string> folderList = Directory.GetDirectories(FolderPath,"*",SearchOption.TopDirectoryOnly).ToList();
            //init managers
            foreach (string folder in folderList)
            {
                Managers.Add(new FolderManager(folder));
            }
        }
        public List<FileOperation> GetFolderChanges(List<string> files)
        {
            List<FileOperation> messages = new List<FileOperation>();
            foreach(string file in files)
            {
                if (_fileList.ContainsKey(file) && File.GetLastWriteTime(file)>_fileList[file])
                {
                    messages.Add(new FileOperation(GetFullFilePath(file), FileAction.Modified));       
                }
                if (!_fileList.ContainsKey(file))
                {
                    messages.Add(new FileOperation(GetFullFilePath(file), FileAction.Created));

                }
            }
            foreach(string file in _fileList.Keys.Except(files)) {
                messages.Add(new FileOperation(GetFullFilePath(file), FileAction.Removed));

            }
            return messages;
        }

        public string GetFullFilePath(string fileName)
        {
            return Path.Combine(FolderPath, fileName);
        }
        public string GetFolderName()
        {
            if (Parent == null) { return ""; }
            return Path.GetRelativePath(Parent.FolderPath, FolderPath);
        }
    }
}
