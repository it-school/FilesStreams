using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace FilesStreams
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShowDrivesInfo();
            ShowDirectoryInfo();
            HandlingDirectory();
            HandlingFileAsync();
            FileStreamsHandling();
            Compressor.Demo(@"c:\tmp\pirates.txt");
            SerializeDemo();
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

        private static void HandlingDirectory()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"c:\tmp\xyz");

            // Creating directories
            dirInfo.Create();

            dirInfo = new DirectoryInfo(@"C:\tmp\");
            dirInfo.CreateSubdirectory("newDir");
            dirInfo.CreateSubdirectory(@"newDir\subDir");

            ShowDirectoryInfo(dirInfo.GetDirectories()[0].FullName);

            // Deleting a directory
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
                }
                catch { }
            }

            // Rename/move directory            
            string oldDirName = @"C:\tmp\newDir";
            if (Directory.Exists(oldDirName))
            {
                Console.WriteLine($"\nPlease enter a new name for directory {oldDirName}:");
                string newDirName = Console.ReadLine();

                if (newDirName != string.Empty)
                {
                    newDirName = @"C:\tmp\" + newDirName;
                    Directory.Move(oldDirName, newDirName);
                    // checking directory has been renamed or not
                    if (Directory.Exists(newDirName) && !Directory.Exists(oldDirName))
                    {
                        Console.WriteLine($"The directory was renamed from {oldDirName} to {newDirName}");
                    }
                }
                else
                {
                    Console.WriteLine($"The directory was NOT renamed");
                }
            }
        }

        private static async Task HandlingFileAsync()
        {
            // ----- SIMPLE TEXT FILE READING & WRITING -----

            string[] pirates = {
                "Ben Gunn", "Billy Bones", "Black Dog", "Blind Pew", "Captain Smollett",
                "Dr. Livesey", "Jim Hawkins", "Long John Silver", "Squire Trelawney",
            };

            string textFilePath = @"C:\tmp\pirates.txt";
            File.WriteAllLines(textFilePath, pirates);
            int number = 0;
            foreach (string pirate in File.ReadAllLines(textFilePath))
            {
                Console.WriteLine($"{++number}) {pirate}");
            }


            string path = @"c:\tmp\content.txt";
            string originalText = "Hello, students!";
            File.WriteAllText(path, originalText);           // запись строки
            File.AppendAllText(path, "\nHello, everybody!"); // дозапись в конец файла
            string fileText = File.ReadAllText(path);        // чтение файла
            Console.WriteLine($"\n\n{fileText}");


            // Encoding use
            Console.WriteLine();
            File.WriteAllText(path, "1) " + originalText, System.Text.Encoding.Unicode);
            File.AppendAllText(path, "\n2) Привет, мир", System.Text.Encoding.Unicode);
            fileText = File.ReadAllText(path, System.Text.Encoding.Unicode);
            Console.WriteLine(fileText);

            File.WriteAllText(path, "\n3) Hello, coding", System.Text.Encoding.GetEncoding("iso-8859-1"));
            fileText = File.ReadAllText(path, System.Text.Encoding.GetEncoding("iso-8859-1"));
            Console.WriteLine(fileText);


            // Write data to file starting from defined position
            path = @"c:\tmp\note.dat";
            string text = "Hello, world!!!";

            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))    // запись в файл
            {
                byte[] input = Encoding.Default.GetBytes(text); // преобразуем строку в байты
                fstream.Write(input, 0, input.Length); // запись массива байтов в файл
            }
            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                string replaceText = "???";         // заменим в файле !!! world на ???
                fstream.Seek(-3, SeekOrigin.End);   // минус 3 символа с конца потока
                byte[] input = Encoding.Default.GetBytes(replaceText);
                await fstream.WriteAsync(input, 0, input.Length);

                fstream.Seek(0, SeekOrigin.Begin);  // считываем весь файл и возвращаем указатель в начало файла
                byte[] output = new byte[fstream.Length];
                await fstream.ReadAsync(output, 0, output.Length);
                string textFromFile = Encoding.Default.GetString(output); // декодируем байты в строку
                Console.WriteLine($"\nОбновлённый текст из файла: {textFromFile}"); // Hello, world???
            }
        }

        private static void FileStreamsHandling()
        {
            string textFilePath1 = @"C:\tmp\testfile1.txt";
            string randString = "This is a random string";

            FileStream fileStream = File.Open(textFilePath1, FileMode.Create);
            byte[] rsByteArray = Encoding.Default.GetBytes(randString); // Convert to a byte array
            byte[] fileByteArray = new byte[rsByteArray.Length];    // Create byte array to hold file data

            fileStream.Write(rsByteArray, 0, rsByteArray.Length);   // Write to file by defining the byte array, the index to start writing from and length
            fileStream.Position = 0;                                // Move back to the beginning of the file
            for (int i = 0; i < rsByteArray.Length; i++) // Put bytes in array
            {
                fileByteArray[i] = (byte)fileStream.ReadByte();
            }
            Console.WriteLine(Encoding.Default.GetString(fileByteArray)); // Convert from bytes to string and output

            fileStream.Close(); // Close the FileStream


            //  STREAMWRITER / STREAMREADER are best for reading and writing strings
            string textFilePath2 = @"C:\tmp\testfile2.txt";

            StreamWriter sw = File.CreateText(textFilePath2); // Create a text file
            sw.Write("This is a random ");  // Write to a file without a newline
            sw.WriteLine("sentence.");
            sw.WriteLine("This is another sentence.");
            sw.Close();                     // Close the StreamWriter

            StreamReader sr = File.OpenText(textFilePath2); // Open the file for reading
            Console.WriteLine("Peek : {0}", Convert.ToChar(sr.Peek())); // Peek returns the next character as a unicode number. Use Convert to change to a character
            Console.WriteLine("1st String : {0}", sr.ReadLine());   // Read to a newline
            Console.WriteLine("Everything : {0}", sr.ReadToEnd());  // Read to the end of the file starting where you left off reading
            sr.Close();

            //  BINARYWRITER / BINARYREADER are used to read and write data types
            string dataFilePath = @"C:\tmp\testfile.dat";

            FileInfo datFile = new FileInfo(dataFilePath);              // Get the file            
            BinaryWriter bw = new BinaryWriter(datFile.OpenWrite());    // Open the file

            // Data to save to the file
            string someText = "Some text in file";
            int myAge = 42;
            double height = 1.75;

            // Write data to a file
            bw.Write(someText);
            bw.Write(myAge);
            bw.Write(height);
            bw.Close();

            BinaryReader br = new BinaryReader(datFile.OpenRead());     // Open file for reading
            // Read and Output data
            Console.WriteLine(br.ReadString());
            Console.WriteLine(br.ReadInt32());
            Console.WriteLine(br.ReadDouble());
            br.Close();
        }

        private static void SerializeDemo()
        {
            Animal animal = new Animal("Bobcat", 15, 45);

            // Serialize the object data to a file
            Stream stream = File.Open("AnimalData.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            // Send the object data to the file
#pragma warning disable SYSLIB0011
            bf.Serialize(stream, animal);
            stream.Close();

            // Force deleting of the animal data
            animal = null;

            // Read object data from the file
            stream = File.Open("AnimalData.dat", FileMode.Open);
            bf = new BinaryFormatter();

            animal = (Animal)bf.Deserialize(stream);
            stream.Close();
#pragma warning restore SYSLIB0011

            Console.WriteLine(animal.ToString());

            // Change animal to show changes were made
            animal.Weight = 50;

            // XmlSerializer writes object data as XML
            XmlSerializer serializer = new XmlSerializer(typeof(Animal));
            using (TextWriter tw = new StreamWriter(@"C:\tmp\animal.xml"))
            {
                serializer.Serialize(tw, animal);
            }

            // Delete animal data
            animal = null;

            // Deserialize from XML to the object
            XmlSerializer deserializer = new XmlSerializer(typeof(Animal));
            TextReader reader = new StreamReader(@"C:\tmp\animal.xml");
            object obj = deserializer.Deserialize(reader);
            animal = (Animal)obj;
            reader.Close();

            Console.WriteLine(animal.ToString());

            // Save a collection of Animals
            List<Animal> theAnimals = new List<Animal>
            {
                new Animal("Dog Mario", 60, 70),
                new Animal("Tiger Luigi", 55, 124),
                new Animal("Kitty Peach", 5, 20)
            };

            using (Stream fs = new FileStream(@"C:\tmp\animals.xml",
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer serializer2 = new XmlSerializer(typeof(List<Animal>));
                serializer2.Serialize(fs, theAnimals);
            }

            // Delete list data
            theAnimals = null;

            // Read data from XML
            XmlSerializer serializer3 = new XmlSerializer(typeof(List<Animal>));

            using (FileStream fs2 = File.OpenRead(@"C:\tmp\animals.xml"))
            {
                theAnimals = (List<Animal>)serializer3.Deserialize(fs2);
            }

            foreach (Animal a in theAnimals)
            {
                Console.WriteLine(a.ToString());
            }
        }
    }
}