using Sortify.Enums;

namespace Sortify.Services;

public class FileSorterService
{
    private static void MoveFilesIntoSubfolder(IGrouping<string?, Models.File> group, string path, Func<Models.File, string> folderNameSelector)
    {
        var groupsByFileExt = group.GroupBy(f => f.Extension);
        
        foreach (var groupByFileExt in groupsByFileExt)
        {
            foreach (var fileWithExt in groupByFileExt)
            {
                var prefix = fileWithExt.Name + fileWithExt.Extension;
                var destinationFolderPath = Path.Combine(path, group.Key!, folderNameSelector(fileWithExt));

                if (!Directory.Exists(destinationFolderPath))
                {
                    Directory.CreateDirectory(destinationFolderPath);
                }

                File.Move(Path.Combine(path, group.Key!, prefix), Path.Combine(destinationFolderPath, prefix));
            }
        }
    }
    
    public static void SortFiles(string path, SortMethod sortMethod)
    {
        var files = Directory.GetFiles(path);
        List<Models.File> files_list = [];
        Dictionary<string, string> extensionToCategory = new () {
            [".jpg"] = "photos",
            [".png"] = "photos",
            [".gif"] = "GIFs",
            [".mov"] = "videos",
            [".mp4"] = "videos",
            [".rar"] = "archives",
            [".7z"] = "archives"
        };

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
            Console.WriteLine($"Group: {group.Key} ({group.Count()} files)");

            Directory.CreateDirectory($"{path + group.Key}");

            foreach (var file in group)
            {
                var prefix = file.Name + file.Extension;
                var destinationFilePath = Path.Combine(path, group.Key!, prefix);
                var destinationFolderPath = Path.Combine(path, group.Key!);

                Console.WriteLine($"{prefix}");

                if (File.Exists(destinationFilePath))
                {
                    continue;
                }

                File.Move(path + prefix, destinationFilePath);
            }

            switch (sortMethod)
            {
                case SortMethod.SortByType:
                    MoveFilesIntoSubfolder(group, path, f => f.Extension![1..]);
                    break;
                case SortMethod.SortByCategory:
                    MoveFilesIntoSubfolder(group, path, f => extensionToCategory.TryGetValue(f.Extension!, out var mappedCategory) ? mappedCategory : "other");
                    break;
                default:
                    break;
            }
        }
    }
}