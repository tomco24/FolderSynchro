using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchro
{
    internal class Synchronizer
    {
        private FolderManager _source;
        private FolderManager _replica;
        public Synchronizer(FolderManager manager,FolderManager replica,string logPath)
        {
            _source = manager;
            _replica = replica;
        }
        public void InitSynchronize()
        {
            _source.InitFileList();
            List<FileMessage> messages = _source.GetFolderChanges(_replica.GetFileList());

        }

        public void Execute(List<FileMessage> messages)
        {
            foreach (FileMessage message in messages)
            {

            }
        }
    }
}
