using System;

namespace Sortify
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: sortify <folder-path> [--sort-by-type]");
                return;
            }

            string folderPath = args[0];
            bool sortByType = args.Contains("--sort-by-type");

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Folder path not found");
                return;
            }

            Console.WriteLine($"Folder path: {folderPath}");
            Console.WriteLine($"Sort by type: {sortByType}");
        }
    }
}