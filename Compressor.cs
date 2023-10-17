using System.IO.Compression;

namespace FilesStreams
{
    internal class Compressor
    {
        public static void Demo(string sourceFile)
        {
            string compressedFile = @"c:\tmp\pirates.txt.gz";   // сжатый файл
            string targetFile = @"c:\tmp\pirates_new.txt";      // восстановленный файл

            Compress(sourceFile, compressedFile);   // создание сжатого файла
            Decompress(compressedFile, targetFile); // чтение из сжатого файла
            ZipFolder(@"c:\tmp\folder");            // упаковка каталога
        }

        private static void ZipFolder(string sourceFolder)
        {
            string zipFile = @"c:\tmp\tmp_folder.zip"; // сжатый файл
            string targetFolder = @"c:\tmp\newfolder"; // папка, куда распаковывается файл

            ZipFile.CreateFromDirectory(sourceFolder, zipFile);
            Console.WriteLine($"Папка {sourceFolder} архивирована в файл {zipFile}");
            ZipFile.ExtractToDirectory(zipFile, targetFolder);

            Console.WriteLine($"Файл {zipFile} распакован в папку {targetFolder}");
        }

        internal static void Compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate);
            // поток для записи сжатого файла
            using FileStream targetStream = File.Create(compressedFile);

            // поток архивации
            using GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress);
            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой

            Console.WriteLine($"Сжатие файла {sourceFile} завершено.");
            Console.WriteLine($"Исходный размер: {sourceStream.Length}  сжатый размер: {targetStream.Length}");
        }

        internal static void Decompress(string compressedFile, string targetFile)
        {
            // поток для чтения из сжатого файла
            using FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate);
            // поток для записи восстановленного файла
            using FileStream targetStream = File.Create(targetFile);
            // поток разархивации
            using GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress);
            decompressionStream.CopyTo(targetStream);
            Console.WriteLine($"Восстановлен файл: {targetFile}");
        }
    }
}
