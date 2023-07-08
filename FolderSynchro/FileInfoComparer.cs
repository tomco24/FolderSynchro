using System.Diagnostics.CodeAnalysis;

namespace FolderSynchro
{
    public class FileInfoComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo? x, FileInfo? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return x.FilePath == y.FilePath &&
                x.WriteTime == y.WriteTime &&
                x.Attributes == y.Attributes;
        }

        public int GetHashCode([DisallowNull] FileInfo obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();

        }
    }
}
