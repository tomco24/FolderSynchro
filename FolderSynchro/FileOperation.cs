using FolderSynchro.enums;

namespace FolderSynchro
{
    internal class FileOperation
    {
        public FileAction Action { get; set; }
        public string FilePath { get; set; }
        public FileOperation(string filename, FileAction fileAction)
        {
            FilePath = filename;
            Action = fileAction;
        }
        public void ExecuteAction(FolderManager manager)
        {

        }

    }
}
