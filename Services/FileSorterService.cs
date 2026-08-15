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
        List<Models.File> filesList = [];
        Dictionary<string, string> extensionToCategory = new () {
            [".jpg"] = "photos",
            [".png"] = "photos",
            [".gif"] = "GIFs",
            [".mov"] = "videos",
            [".mp4"] = "videos",
            [".zip"] = "archives",
            [".rar"] = "archives",
            [".7z"] = "archives"
        };

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

            filesList.Add(fileModel);
        }

        var groups = filesList.GroupBy(f => f.Name?[..f.DashIndex]);

        foreach (var group in groups)
        {
            Console.WriteLine($"Group: {group.Key} ({group.Count()} files)");

            Directory.CreateDirectory(Path.Combine(path, group.Key!));

            foreach (var file in group)
            {
                var prefix = file.Name + file.Extension;
                var destinationFilePath = Path.Combine(path, group.Key!, prefix);

                Console.WriteLine($"{prefix}");

                if (File.Exists(destinationFilePath))
                {
                    continue;
                }

                File.Move(Path.Combine(path, prefix), destinationFilePath);
            }

            switch (sortMethod)
            {
                case SortMethod.SortByType:
                    MoveFilesIntoSubfolder(group, path, f => f.Extension!.ToLower()[1..]);
                    break;
                case SortMethod.SortByCategory:
                    MoveFilesIntoSubfolder(group, path, f => extensionToCategory.TryGetValue(f.Extension!.ToLower(), out var mappedCategory) ? mappedCategory : "other");
                    break;
                default:
                    break;
            }
        }
    }
}