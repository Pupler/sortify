namespace Sortify.Services;

public class FileSorterService
{
    public static void SortFiles(string path)
    {
        var files = Directory.GetFiles(path);

        foreach (var file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            var indexLocated = fileName.IndexOf("-");

            if (indexLocated == -1)
            {
                Console.WriteLine($"{fileName}: Index \'-\' not found");

                continue;
            }
        }
    }
}