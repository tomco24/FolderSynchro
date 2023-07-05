using FolderSynchro.enums;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchro
{
    internal class FileMessage
    {
        public FileAction Action { get; set; }
        public string FileName { get; set; }
        public FileMessage(string filename,FileAction fileAction)
        {
            FileName = filename;
            Action = fileAction;
        }

    }
}
