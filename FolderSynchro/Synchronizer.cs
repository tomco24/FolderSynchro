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

        public void Equalize()
        {
            List<FileOperation> operations = _replica.GetFolderChanges(_source.GetFileList());

        }
        public void Synchronize()
        {
            //todo must fill init file list
            if (!Directory.Exists(_replica.FolderPath))
            {
                Directory.CreateDirectory(_replica.FolderPath);
            }

            List<FileOperation> operations = _replica.GetFolderChanges(_source.GetFileList());
            Execute(operations);


        }

        private void SynchronizeSubFolders(FolderManager source, FolderManager replica)  
        {
            bool match = false;
            FolderManagerComparer comparer = new FolderManagerComparer();
            //List<FolderManager> inBoth = source.Managers.Intersect(replica.Managers,comparer).ToList();
            //List<FolderManager> inSource = source.Managers.Except(replica.Managers,comparer).ToList();
            List<FolderManager> inReplica = replica.Managers.Except(source.Managers,comparer).ToList();

            foreach (FolderManager manager in source.Managers)
            {
                foreach (FolderManager replicaManager in replica.Managers)
                {
                    if (comparer.Equals(manager,replicaManager))
                    {
                        SynchronizeManagers(manager, replicaManager);
                        match = true;
                        break;
                    }
                }
                if (match)
                {
                    FolderManager createdManager = CreateFolder(manager, replica);
                    SynchronizeManagers(manager, createdManager);
                    match = false;
                }
            }
            DeleteFolders(inReplica);

        }

        private void SynchronizeManagers(FolderManager source,FolderManager replica)
        {
            List<FileOperation> operations = _replica.GetFolderChanges(_source.GetFileList());
            Execute(operations);
            if (source.Managers.Count > 0 || replica.Managers.Count >0)
            {
                SynchronizeSubFolders(source, replica);
            }

        }

        public void Execute(List<FileOperation> operations)
        {
            foreach (FileOperation operation in operations)
            {
                operation.ExecuteAction(_replica);
                // add logging
            }
        }

        public FolderManager CreateFolder(FolderManager source,FolderManager replica)
        {
            FolderManager createdManager = new FolderManager(replica, source.FolderName);
            replica.Managers.Add(createdManager);
            Directory.CreateDirectory(createdManager.FolderPath);
            return createdManager;
        }

        private void DeleteFolders(List<FolderManager> managers)
        {
            foreach (FolderManager manager in managers)
            {
                Directory.Delete(manager.FolderPath, true);
            }
        }
    }
}
