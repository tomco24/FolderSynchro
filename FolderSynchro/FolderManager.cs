using FolderSynchro.enums;

namespace FolderSynchro
{
    public class FolderManager
    {
        public string FolderPath { get; }

        private List<FolderManager> _managers = new List<FolderManager>();
        private Dictionary<string, FileInfo> _fileList = new Dictionary<string, FileInfo>();
        public FolderManager(string path)
        {
            // TODO check path
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FolderPath = path;
        }

        public List<FileInfo> GetFileInfoList()
        {
            return Directory.GetFiles(FolderPath, "*", SearchOption.AllDirectories)
                .Select(f => FileInfo.CreateInstance(f)).ToList();
        }
        public List<String> GetFileList()
        {
            return Directory.GetFiles(FolderPath, "*", SearchOption.AllDirectories).ToList();
        }
        public List<string> GetFolderList()
        {
            return Directory.GetDirectories(FolderPath, "*", SearchOption.AllDirectories).ToList();
        }
        public void InitFileList()
        {
            _fileList = GetFileInfoList().ToDictionary(f => f.FilePath, f => f);

        }
        public List<FileOperation> GetFolderChanges(List<FileInfo> files)
        {
            List<FileOperation> messages = new List<FileOperation>();
            FileInfoComparer comparer = new FileInfoComparer();
            foreach (FileInfo file in files)
            {
                if (_fileList.ContainsKey(file.FilePath) && !comparer.Equals(file, _fileList[file.FilePath]))
                {
                    messages.Add(new FileOperation(file.FilePath, FileAction.Modified));
                    _fileList[file.FilePath] = file;
                }
                if (!_fileList.ContainsKey(file.FilePath))
                {
                    messages.Add(new FileOperation(file.FilePath, FileAction.Created));
                    _fileList[file.FilePath] = file;

                }
            }
            foreach (string file in _fileList.Keys.Except(files.Select(x => x.FilePath).ToList()))
            {
                messages.Add(new FileOperation(file, FileAction.Removed));
                _fileList.Remove(file);

            }
            return messages;
        }

        public string GetFullFilePath(string fileName)
        {
            return Path.Combine(FolderPath, fileName);
        }
    }
}
