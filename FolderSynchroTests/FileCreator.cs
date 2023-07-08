namespace FolderSynchroTests
{
    internal class FileCreator
    {
        private Random random;

        public FileCreator()
        {
            random = new Random();
        }

        public void CreateFile(string path, string name)
        {
            string fullPath = Path.Combine(path, name);

            // If the file already exists, this will overwrite it 
            using (StreamWriter sw = File.CreateText(fullPath))
            {
                // Write a random number to the file
                sw.WriteLine(random.Next());
            }
        }
        public void EditFile(string path, string name)
        {
            string fullPath = Path.Combine(path, name);

            if (File.Exists(fullPath))
            {
                using (StreamWriter sw = new StreamWriter(fullPath, false))
                {
                    sw.WriteLine(random.Next());
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }

        public void DeleteFile(string path, string name)
        {
            string fullPath = Path.Combine(path, name);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }
        public string CreateDirectory(string path, string name)
        {
            string fullPath = Path.Combine(path, name);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            else
            {
                Console.WriteLine("Directory already exists.");
            }
            return fullPath;
        }

        public void DeleteDirectory(string path, string name)
        {
            string fullPath = Path.Combine(path, name);

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true); // true = remove contents as well as directory
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
            }
        }

    }
}
