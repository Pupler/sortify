namespace Sortify.Services;

public class FileSorterService
{
    public static void SortFiles(string path)
    {
        var files = Directory.GetFiles(path);
        List<Models.File> files_list = [];

        foreach (var file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            int dashIndex = fileName.IndexOf("-");

            if (dashIndex == -1)
            {
                Console.WriteLine($"{fileName}: Index \'-\' not found");

                continue;
            }

            var fileModel = new Models.File
            {
                Name = fileName,
                DashIndex = dashIndex
            };

            files_list.Add(fileModel);
        }
    }
}