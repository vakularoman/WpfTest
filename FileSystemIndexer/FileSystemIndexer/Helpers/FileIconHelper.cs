using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileSystemIndexer.Helpers
{
    public static class FileIconHelper
    {
        private static Dictionary<string, ImageSource> _cash = new();
        private const string ExeExtension = ".exe";

        public static ImageSource DoSmth(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (!_cash.TryGetValue(extension, out var result))
            {
                var icon = Icon.ExtractAssociatedIcon(filePath);

                if (icon is null)
                {
                    return null;
                }

                result = ToImageSource(icon);

                if (!extension.Equals(ExeExtension))
                {
                    _cash.Add(extension, result);
                }
            }

            return result;
        }

        private static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
