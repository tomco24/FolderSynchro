using FolderSynchro;
using FolderSynchro.enums;

// Display the number of command line arguments.
if (args.Length < 4)
{
    Console.WriteLine("Not enough parameters!");
    return;
}
string sourcePath = args[0];
string replicaPath = args[1];
int interval = int.Parse(args[2]);
string logPath = args[3];
FolderManagerFactory factory = new FolderManagerFactory();
FolderManager manager = factory.GetFolderManager(sourcePath,FolderType.Source);
manager.InitFileList();
Console.WriteLine("End");
