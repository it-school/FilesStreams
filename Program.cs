namespace FilesStreams
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShowDrivesInfo();
            ShowDirectoryInfo();
            ManageDirectory();
        }

        private static void ShowDrivesInfo()
        {
            Console.WriteLine("******* Drives Info *******\n");

            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            foreach (DriveInfo drive in driveInfo)
            {
                try
                {
                    if (drive.IsReady)
                        Console.WriteLine($"{drive.Name}\tType: {drive.DriveType}\tFileSystem: {drive.DriveFormat}\tLabel: {drive.VolumeLabel}\tFree space: {drive.TotalFreeSpace >> 30} of {drive.TotalSize >> 30} GB");
                    else
                        Console.WriteLine($"{drive.Name}\tdrive is not ready");
                }
                catch { }
            }
        }

        private static void ShowDirectoryInfo(string path = @"C:\tmp")
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            Console.WriteLine("\n\n******* Directory Info *******\n");
            Console.WriteLine($"Full Name: {dirInfo.FullName}");
            Console.WriteLine($"Root: {dirInfo.Root}");
            Console.WriteLine($"Parent: {dirInfo.Parent}");
            Console.WriteLine($"Name: {dirInfo.Name}");
            Console.WriteLine($"Attributes: {dirInfo.Attributes}");
            Console.WriteLine($"Creation Time: {dirInfo.CreationTime}");
        }

        private static void ManageDirectory()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"c:\tmp\xyz");
            dirInfo.Create();

            dirInfo = new DirectoryInfo(@"C:\tmp\");
            dirInfo.CreateSubdirectory("newDir");
            dirInfo.CreateSubdirectory(@"newDir\subDir");

            ShowDirectoryInfo(dirInfo.GetDirectories()[0].FullName);

            dirInfo = new DirectoryInfo(@"c:\tmp\xyz");
            Console.Write($"\nAre you sure to Delete {dirInfo.FullName}?\t");
            string str = Console.ReadLine();
            if (str.ToLower() == "y")
            {
                try
                {
                    Directory.Delete(@"c:\tmp\xyz", true);
                    Console.Write($"{dirInfo.FullName} is Deleted.....");
                    ShowDirectoryInfo(@"c:\tmp\xyz");
                } catch { }
            }            
        }
    }
}