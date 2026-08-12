using Sortify.Enums;

namespace Sortify.Services;

public class FileSorterService
{
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
            if (group.Key != null)
            {
                Console.WriteLine(group.Key);

                Directory.CreateDirectory($"{path + group.Key}");

                foreach (var file in group)
                {
                    var prefix = file.Name + file.Extension;
                    var destinationFilePath = Path.Combine(path, group.Key, prefix);
                    var destinationFolderPath = Path.Combine(path, group.Key);

                    Console.WriteLine($"{file.Name}: {file.DashIndex}");

                    if (File.Exists(destinationFilePath))
                    {
                        continue;
                    }

                    File.Move(path + prefix, destinationFilePath);
                }

                switch (sortMethod)
                {
                    case SortMethod.SortByType:
                        var groupsByFileExt = group.GroupBy(f => f.Extension);

                        foreach (var groupExt in groupsByFileExt)
                        {
                            foreach (var fileWithExt in groupExt)
                            {
                                var prefix = fileWithExt.Name + fileWithExt.Extension;
                                var destinationFolderPath = Path.Combine(path, group.Key, groupExt.Key[1..]);

                                if (!Directory.Exists(destinationFolderPath))
                                {
                                    Directory.CreateDirectory(destinationFolderPath);
                                }

                                File.Move(Path.Combine(path, group.Key, prefix), Path.Combine(destinationFolderPath, prefix));
                            }
                        }
                        break;
                    case SortMethod.SortByCategory:
                        var groupsByFileExtension = group.GroupBy(f => f.Extension);

                        foreach (var groupByFileExt in groupsByFileExtension)
                        {
                            foreach (var fileWithExt in groupByFileExt)
                            {
                                var prefix = fileWithExt.Name + fileWithExt.Extension;
                                
                                if (extensionToCategory.TryGetValue(fileWithExt.Extension, out var category))
                                {
                                    var destFolderPath = Path.Combine(path, group.Key, category);

                                    if (!Directory.Exists(destFolderPath))
                                    {
                                        Directory.CreateDirectory(destFolderPath);
                                    }

                                    File.Move(Path.Combine(path, group.Key, prefix), Path.Combine(destFolderPath, prefix));
                                }
                                else
                                {
                                    var destFolderPath = Path.Combine(path, group.Key, "other");

                                    if (!Directory.Exists(destFolderPath))
                                    {
                                        Directory.CreateDirectory(destFolderPath);
                                    }

                                    File.Move(Path.Combine(path, group.Key, prefix), Path.Combine(destFolderPath, prefix));
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}