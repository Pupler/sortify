namespace Sortify.Services;

public class FileSorterService
{
    public static void SortFiles(string path)
    {
        var files = Directory.GetFiles(path);
        List<Models.File> files_list = [];

        if (path[^1] != '/')
        {
            path += '/';
        }

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
            if (group.Key != null)
            {
                Console.WriteLine(group.Key);

                Directory.CreateDirectory($"{path + group.Key}");

                foreach (var file in group)
                {
                    var prefix = file.Name + file.Extension;
                    var destinationFolderPath = Path.Combine(path, group.Key, prefix);

                    Console.WriteLine($"{file.Name}: {file.DashIndex}");

                    if (File.Exists(destinationFolderPath))
                    {
                        continue;
                    }

                    File.Move(path + prefix, destinationFolderPath);
                }
            }
        }
    }
}