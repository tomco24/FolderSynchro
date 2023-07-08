using FolderSynchro.enums;

namespace FolderSynchro
{
    public class FileOperation
    {
        public FileAction Action { get; }
        public string FilePath { get; }
        public FileOperation(string filename, FileAction fileAction)
        {
            FilePath = filename;
            Action = fileAction;
        }

    }
}
