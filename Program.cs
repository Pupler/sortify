using Sortify.Enums;
using Sortify.Services;

namespace Sortify;

class Program()
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args.Length > 2)
        {
            Console.WriteLine("Usage: sortify <folder-path> [--sort-by-type | --sort-by-category]");
            return;
        }

        var sortMethod = SortMethod.None;
        string folderPath = args[0];
        bool sortByType = args.Contains("--sort-by-type");
        bool sortByCategory = args.Contains("--sort-by-category");

        if (sortByType)
        {
            sortMethod = SortMethod.SortByType;
        }
        else if (sortByCategory)
        {
            sortMethod = SortMethod.SortByCategory;
        }

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Folder path not found");
            return;
        }

        Console.WriteLine($"Folder path: {folderPath}");
        Console.WriteLine($"Sort by: {sortMethod}");

        FileSorterService.SortFiles(folderPath, sortMethod);
    }
}