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
        FolderManager source = new FolderManager(sourcePath);
        FolderManager replica = new FolderManager(replicaPath);

        source.InitFileList();
        source.InitManagers();
        Console.WriteLine(Path.GetRelativePath(source.FolderPath, source.Managers[0].FolderPath));
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