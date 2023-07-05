using FolderSynchro.enums;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchro
{
    internal class FileOperation
    {
        public FileAction Action { get; set; }
        public string FileName { get; set; }
        public FileOperation(string filename,FileAction fileAction)
        {
            FileName = filename;
            Action = fileAction;
        }
        public void ExecuteAction(FolderManager manager)
        {
            string destPath;
            string name;
            if (Action == FileAction.Modified || Action == FileAction.Created)
            {
                destPath = manager.GetFullFilePath(FileName);
                File.Copy(FileName, destPath, true);
            }
            else
            {
                name = Path.GetFileName(FileName);
                destPath = manager.GetFullFilePath(name);
                if (File.Exists(destPath))
                {
                    File.Delete(destPath);
                }
            }
        }

    }
}
