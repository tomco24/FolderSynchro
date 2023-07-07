using FolderSynchro;
using NLog;

internal class Program


{
    private static void SetupLog(string logPath)
    {

        NLog.LogManager.Setup().LoadConfiguration(builder =>
        {
            builder.ForLogger().FilterMinLevel(LogLevel.Info).WriteToConsole();
            builder.ForLogger().FilterMinLevel(LogLevel.Info).WriteToFile(fileName: logPath);
        });

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
        SetupLog(logPath);
        FolderManagerFactory factory = new FolderManagerFactory();
        FolderManager source = new FolderManager(sourcePath);
        FolderManager replica = new FolderManager(replicaPath);
        Synchronizer synchronizer = new Synchronizer(source, replica, logPath);
        synchronizer.Equalize();
        source.InitFileList();
        while (true)
        {
            try
            {
                Thread.Sleep(interval);
                Console.WriteLine("End");
                synchronizer.Synchronize();

            }
            catch { }
        }
    }
}