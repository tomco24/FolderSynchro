using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchro.enums
{
    public enum FolderType
    {
        Source,
        Replica
    }
    public enum FileAction
    {
        Modified,
        Removed,
        Created
    }
}
