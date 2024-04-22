using System.IO;

namespace FileSystemIndexer.Helpers
{
    public static class FileSystemIterator
    {
        public static IEnumerable<string> Iterate(string rootDirectory)
        {
            var files = Enumerable.Empty<string>();
            var directories = Enumerable.Empty<string>();

            try
            {
                files = Directory.GetFiles(rootDirectory);
                directories = Directory.GetDirectories(rootDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore
            }

            foreach (var file in files)
            {
                yield return file;
            }

            var subdirectoryItems = directories.SelectMany(Iterate);

            foreach (var result in subdirectoryItems)
            {
                yield return result;
            }
        }
    }
}
