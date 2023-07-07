namespace FolderSynchro
{
    internal class FolderManagerFactory
    {
        public FolderManagerFactory() { }
        public FolderManager GetFolderManager(string path, enums.FolderType category)
        {
            if (category == enums.FolderType.Source)
            {
                return new FolderManager(path);
            }
            else
            {
                return new FolderManager(path);
            }
        }
    }
}
