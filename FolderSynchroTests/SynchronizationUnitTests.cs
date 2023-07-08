namespace FolderSynchroTests
{
    [TestClass]
    public class SynchronizationUnitTests
    {
        Checker checker;
        FileCreator creator;
        public const string baseSource = "./data/source";
        public const string baseReplica = "./data/replica";
        [TestInitialize]
        public void Initialize()
        {
            checker = new Checker();
            creator = new FileCreator();
            Directory.CreateDirectory("./data");
            Directory.CreateDirectory(baseSource);
            Directory.CreateDirectory(baseReplica);
        }
        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete("./data", true);
        }
        [TestMethod]
        public void TestFolderEqualize()
        {
            // Set up the source folder with sample content

            creator.CreateFile(baseSource, "A.txt");
            creator.CreateFile(baseSource, "B.txt");
            var p1 = creator.CreateDirectory(baseSource, "FA");
            creator.CreateFile(p1, "C.txt");
            var p2 = creator.CreateDirectory(p1, "FB");
            creator.CreateFile(p2, "D.txt");


            // Run the program to synchronize the folders
            FolderManager source = new FolderManager(baseSource);
            FolderManager replica = new FolderManager(baseReplica);
            Synchronizer synchronizer = new Synchronizer(source, replica, "log.txt");
            synchronizer.Equalize();
            synchronizer.Synchronize();

            // Verify that the replica folder matches the content of the source folder
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));
        }
        [TestMethod]
        public void TestFolderSynchronization()
        {
            creator.CreateFile(baseSource, "A.txt");
            creator.CreateFile(baseSource, "B.txt");
            var p1 = creator.CreateDirectory(baseSource, "FA");
            creator.CreateFile(p1, "C.txt");
            var p2 = creator.CreateDirectory(p1, "FB");
            creator.CreateFile(p2, "D.txt");
            FolderManager source = new FolderManager(baseSource);
            FolderManager replica = new FolderManager(baseReplica);
            Synchronizer synchronizer = new Synchronizer(source, replica, "log.txt");
            synchronizer.Equalize();
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

            // add files
            creator.CreateFile(baseSource, "E.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));


            creator.CreateFile(p1, "F.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

            //modify files
            creator.EditFile(baseSource, "E.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));


            creator.EditFile(p1, "F.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));


            //delete files
            creator.DeleteFile(baseSource, "E.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));


            creator.DeleteFile(p1, "F.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));


        }

        [TestMethod]
        public void TestSubFoldersHandling()
        {
            creator.CreateFile(baseSource, "A.txt");
            creator.CreateFile(baseSource, "B.txt");
            var p1 = creator.CreateDirectory(baseSource, "FA");
            creator.CreateFile(p1, "C.txt");
            var p2 = creator.CreateDirectory(p1, "FB");
            creator.CreateFile(p2, "D.txt");
            FolderManager source = new FolderManager(baseSource);
            FolderManager replica = new FolderManager(baseReplica);
            Synchronizer synchronizer = new Synchronizer(source, replica, "log.txt");
            synchronizer.Equalize();
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

            // adding more folders and files
            var p3 = creator.CreateDirectory(p1, "FC");
            var p4 = creator.CreateDirectory(p1, "FD");
            creator.CreateFile(p3, "H.txt");
            creator.CreateFile(p3, "I.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

            // removing empty folder
            creator.DeleteDirectory(p1, "FD");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

            //removing not empty folder
            creator.DeleteDirectory(p1, "FC");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

        }

        [TestMethod]
        public void TestReplicaChanges()
        {
            creator.CreateFile(baseSource, "A.txt");
            creator.CreateFile(baseSource, "B.txt");
            var p1 = creator.CreateDirectory(baseSource, "FA");
            creator.CreateFile(p1, "C.txt");
            var p2 = creator.CreateDirectory(p1, "FB");
            creator.CreateFile(p2, "D.txt");
            FolderManager source = new FolderManager(baseSource);
            FolderManager replica = new FolderManager(baseReplica);
            Synchronizer synchronizer = new Synchronizer(source, replica, "log.txt");
            synchronizer.Equalize();
            synchronizer.Synchronize();

            // creating files in replica
            creator.CreateFile(baseReplica, "RA.txt");
            creator.CreateFile(baseReplica, "RB.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

            //deleting files in replica
            creator.DeleteFile(baseReplica, "A.txt");
            creator.DeleteFile(baseReplica, "B.txt");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

            //creating folder in repica
            creator.CreateFile(baseReplica, "RF");
            creator.CreateFile(baseReplica, "RG");
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));

        }

        [TestMethod]
        public void TestLargeFolders()
        {
            creator.CreateFile(baseSource, "A.txt");
            creator.CreateFile(baseSource, "B.txt");
            var p1 = creator.CreateDirectory(baseSource, "FA");
            creator.CreateFile(p1, "C.txt");
            var p2 = creator.CreateDirectory(p1, "FB");
            creator.CreateFile(p2, "D.txt");
            FolderManager source = new FolderManager(baseSource);
            FolderManager replica = new FolderManager(baseReplica);
            Synchronizer synchronizer = new Synchronizer(source, replica, "log.txt");
            synchronizer.Equalize();
            synchronizer.Synchronize();

            for (int i = 0; i < 10000; i++)
            {
                creator.CreateFile(p1, $"file_{i}.txt");
            }
            synchronizer.Synchronize();
            Assert.IsTrue(checker.CheckContents(baseSource, baseReplica));


        }

    }
}