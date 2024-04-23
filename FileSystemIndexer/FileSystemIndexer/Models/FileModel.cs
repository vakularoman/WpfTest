using System.IO;
using System.Windows.Media;
using FileSystemIndexer.Helpers;
using FileSystemIndexer.ViewModels;

namespace FileSystemIndexer.Models
{
    public class FileModel : ObservableObjectBase
    {
        public FileModel(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                var attributes = File.GetAttributes(filePath);
                Icon = FileIconHelper.ResolveIcon(filePath);

                IsHidden = (attributes & FileAttributes.Hidden) != 0;
                IsReadOnly = fileInfo.IsReadOnly;
                Size = fileInfo.Length;
                Path = filePath;
            }
        }

        public bool IsReadOnly { get; }

        public bool IsHidden { get; }

        public long Size { get; }

        public string Path { get; }

        public ImageSource Icon { get; }
    }
}
