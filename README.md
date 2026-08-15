# Sortify

A .NET console tool that sorts files into folders based on the prefix in the filename, with optional secondary sorting by file type or category.

## How it works

Sortify takes all files in a given folder, determines each file's prefix (the part of the name **before the first dash**), and moves files sharing the same prefix into a common subfolder.

Example:

```
lena-1.jpg
lena-2.png
hello-1.txt
```

becomes:

```
lena/
  lena-1.jpg
  lena-2.png
hello/
  hello-1.txt
```

Files whose name has no dash are left untouched.

## Installation

Sortify is installed as a **.NET Global Tool**.

```bash
git clone https://github.com/Pupler/sortify.git
cd sortify
dotnet pack -c Release
dotnet tool install --global --add-source ./nupkg Sortify
```

After installation, the `sortify` command is available globally in the terminal.

To update after changing the code:

```bash
dotnet pack -c Release
dotnet tool update --global --add-source ./nupkg Sortify
```

## Usage

```bash
sortify <folder-path> [--sort-by-type | --sort-by-category]
```

**Examples:**

```bash
# Simple sorting by prefix
sortify ~/Downloads

# Additionally sort each group by file extension
sortify ~/Downloads --sort-by-type

# Additionally sort each group by category (photos/videos/archives/other)
sortify ~/Downloads --sort-by-category
```

## Sorting modes

### `--sort-by-type`

Files within each prefix group are further split into subfolders named after their extension:

```
lena/
  jpg/
    lena-1.jpg
  png/
    lena-2.png
  zip/
    lena-3.zip
```

### `--sort-by-category`

Files are grouped by a meaningful category instead of the raw extension:

| Extension | Category |
|---|---|
| `.jpg`, `.png` | `photos` |
| `.gif` | `GIFs` |
| `.mov`, `.mp4` | `videos` |
| `.rar`, `.7z`, `.zip` | `archives` |
| other | `other` |

```
lena/
  photos/
    lena-1.jpg
    lena-2.png
  archives/
    lena-3.zip
```

## Collision handling

If a file with the same name already exists in the target folder, Sortify does **not** overwrite it — the file stays in its original location, and a message is printed to the console noting the skip.

## Limitations

- The prefix separator is only the `-` (dash) character. Files without a dash in their name are not sorted.
- Files whose name consists only of a dot and an extension (e.g. `.zip`) are also left untouched.
- Extension comparison is case-sensitive (`.JPG` and `.jpg` may end up in different groups).

## Tech stack

- .NET / C#
- LINQ (`GroupBy`, `TryGetValue`)
- Implemented as a .NET Global Tool
