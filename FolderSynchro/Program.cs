using FolderSynchro;
using FolderSynchro.enums;

internal class Program


{
    private void Synchronize(FolderManager source, FolderManager replica)
    {
        //todo: add log 


    }
    private static void Main(string[] args)
    {
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
        FolderManager source = factory.GetFolderManager(sourcePath, FolderType.Source);
        source.InitFileList();
        while (true)
        {
            try
            {
                Thread.Sleep(interval);
                Console.WriteLine("End");

            }
            catch { }
        }
    }
}