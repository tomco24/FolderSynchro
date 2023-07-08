namespace FolderSynchroTests
{
    internal class Checker
    {
        public Checker() { }
        public bool CheckContents(string source, string replica)
        {
            var sourceFiles = Directory.GetFiles(source, "*", SearchOption.AllDirectories).ToList();
            var replicaFiles = Directory.GetFiles(replica, "*", SearchOption.AllDirectories).ToList();
            var sourceFilesSet = sourceFiles.Select(s => Path.GetRelativePath(source, s)).ToHashSet();
            var replicaFilesSet = replicaFiles.Select(s => Path.GetRelativePath(replica, s)).ToHashSet();
            return sourceFilesSet.SetEquals(replicaFilesSet);
        }
    }
}
