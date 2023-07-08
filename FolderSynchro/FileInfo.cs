namespace FolderSynchro
{



    public class FileInfo
    {
        public string FilePath { get; }
        public FileAttributes Attributes { get; }
        public DateTime WriteTime { get; }

        public FileInfo(string path, FileAttributes attributes, DateTime writeTime)
        {
            FilePath = path;
            Attributes = attributes;
            WriteTime = writeTime;
        }


        public static FileInfo CreateInstance(string path)
        {
            return new FileInfo(path, File.GetAttributes(path), File.GetLastWriteTime(path));
        }
    }
}
