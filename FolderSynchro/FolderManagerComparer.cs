﻿using System.Diagnostics.CodeAnalysis;

namespace FolderSynchro
{
    internal class FolderManagerComparer : IEqualityComparer<FolderManager>
    {
        public bool Equals(FolderManager? x, FolderManager? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return x.FolderName == y.FolderName;

        }

        public int GetHashCode([DisallowNull] FolderManager obj)
        {
            if (obj == null) return 0;
            return obj.FolderName.GetHashCode();
        }
    }
}
