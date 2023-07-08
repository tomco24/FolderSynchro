using FolderSynchro;
using NLog;
using System.Diagnostics.CodeAnalysis;

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
    private static bool CheckArguments(string[] args)
    {
        if (args.Length < 4)
        {
            Console.WriteLine("Not enough parameters!");
            return false;
        }
        int interval;
        if (!int.TryParse(args[2], out interval))
        {
            Console.WriteLine("Invalid interval!");
            return false;
        }
        if (interval < 0)
        {
            Console.WriteLine("Interval can't be negative!");
            return false;
        }
        string logPath = args[3];
        string directoryName = Path.GetDirectoryName(logPath);

        if (!Directory.Exists(directoryName))
        {
            Console.WriteLine("Log path does not exist!");
            return false;
        }
        return true;

    }
    private static void Main(string[] args)
    {
        bool argumentsValid = CheckArguments(args);
        if (!argumentsValid) return;
        
        string sourcePath = args[0];
        string replicaPath = args[1];
        int interval = int.Parse(args[2]);
        string logPath = args[3];

        SetupLog(logPath);

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
                synchronizer.Synchronize();

            }
            catch { }
        }
    }
}