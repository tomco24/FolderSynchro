﻿using FolderSynchro.enums;

namespace FolderSynchro
{
    public class Synchronizer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private FolderManager _source;
        private FolderManager _replica;
        public Synchronizer(FolderManager manager, FolderManager replica, string logPath)
        {
            _source = manager;
            _replica = replica;
        }

        public void Equalize()
        {
            if (!Directory.Exists(_replica.FolderPath))
            {
                Directory.CreateDirectory(_replica.FolderPath);
                Logger.Info("Creating replica folder at {0}", _replica.FolderPath);
            }
            SynchronizeFolderStructure();
            EqualizeFiles();
            _source.InitFileList();



        }
        public void Synchronize()
        {
            //todo must fill init file list

            Logger.Info("Synchronization started!");
            SynchronizeFolderStructure();
            SynchronizeFiles();
            if (_source.GetFileList().Count != _replica.GetFileList().Count)
            {
                Equalize();
            }
            Logger.Info("Synchronization finished!");


        }

        private void SynchronizeFiles()
        {
            List<FileOperation> operations = _source.GetFolderChanges(_source.GetFileInfoList());
            Execute(operations);

        }
        private void EqualizeFiles()
        {
            List<string> sourceFiles = _source.GetFileList();
            HashSet<string> replicaFiles = _replica.GetFileList().ToHashSet();

            foreach (string file in sourceFiles)
            {
                string destPath = Path.Combine(_replica.FolderPath, Path.GetRelativePath(_source.FolderPath, file));
                if (!File.Exists(destPath))
                {
                    File.Copy(file, destPath);
                    Logger.Info("Copying {0} into replica {1} folder", file, _replica.FolderPath);
                }
                else
                {
                    replicaFiles.Remove(destPath);
                }
            }
            foreach (string replicaFile in replicaFiles)
            {
                File.Delete(replicaFile);
            }

        }

        private void SynchronizeFolderStructure()
        {
            List<string> replicaFolders = _replica.GetFolderList();
            List<string> replicaSourceFolders = GetReplicaFolderPaths();
            replicaFolders = replicaFolders.Except(replicaSourceFolders).ToList();
            DeleteFolders(replicaFolders);
            foreach (string replicaFolder in replicaSourceFolders)
            {
                if (!Directory.Exists(replicaFolder))
                {
                    Directory.CreateDirectory(replicaFolder);
                    Logger.Info("New folder {} was created in the source folder",
                        Path.GetRelativePath(_replica.FolderPath, replicaFolder));
                }
            }
        }


        private void Execute(List<FileOperation> operations)
        {
            foreach (FileOperation operation in operations)
            {
                ExecuteOperation(operation);
            }
        }

        private void ExecuteOperation(FileOperation operation)
        {
            string destPath;
            destPath = _replica.GetFullFilePath(Path.GetRelativePath(_source.FolderPath, operation.FilePath));
            string actionString = Enum.GetName(typeof(FileAction), operation.Action);

            if (operation.Action == FileAction.Modified || operation.Action == FileAction.Created)
            {
                File.Copy(operation.FilePath, destPath, true);
                Logger.Info("{0} was {1}. Copying from the source folder to the replica folder", operation.FilePath, actionString);
            }
            else
            {
                if (File.Exists(destPath))
                {
                    File.Delete(destPath);
                    Logger.Info("{0} was {1} from the source folder. Removing from the replica folder.", operation.FilePath, actionString);

                }
            }
        }

        private void DeleteFolders(List<string> folders)
        {
            foreach (string folder in folders)
            {
                Directory.Delete(folder, true);
                Logger.Info("Folder {0} was removed from the source folder.",
                    Path.GetRelativePath(_replica.FolderPath, folder));
            }
        }

        private List<string> GetReplicaFolderPaths()
        {
            List<String> folders = _source.GetFolderList();
            folders.Sort();
            for (int i = 0; i < folders.Count; i++)
            {
                String relativePath = Path.GetRelativePath(_source.FolderPath, folders[i]);
                folders[i] = Path.Combine(_replica.FolderPath, relativePath);
            }
            return folders;
        }

    }
}
