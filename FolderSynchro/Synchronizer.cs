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
        public void Synchronize()
        {
            //todo must fill init file list

            List<FileOperation> operations = _source.GetFolderChanges(_source.GetFileList());
            Execute(operations);

        }

        public void Execute(List<FileOperation> operations)
        {
            foreach (FileOperation operation in operations)
            {
                operation.ExecuteAction(_replica);
                // add logging
            }
        }
    }
}
