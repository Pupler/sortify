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
                DashIndex = dashIndex,
                Extension = Path.GetExtension(file)
            };

            files_list.Add(fileModel);
        }

        var groups = files_list.GroupBy(f => f.Name?[..f.DashIndex]);

        foreach (var group in groups)
        {
            Console.WriteLine(group.Key);

            if (path[^1] != '/')
            {
                path += '/';
            }

            Directory.CreateDirectory($"{path + group.Key}");

            foreach (var file in group)
            {
                Console.WriteLine($"{file.Name}: {file.DashIndex}");

                File.Move(path + file.Name + file.Extension, path + group.Key + '/' + file.Name + file.Extension);
            }
        }
    }
}