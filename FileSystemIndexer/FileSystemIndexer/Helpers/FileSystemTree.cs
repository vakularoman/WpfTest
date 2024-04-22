using System.IO;

namespace FileSystemIndexer.Helpers
{
    public class FileSystemTree
    {
        public FileSystemNode Root = new(string.Empty, null);

        public void AddFile(string filePath)
        {
            var pathElements = filePath.Split(Path.DirectorySeparatorChar);

            var currentParent = Root;

            foreach (var pathElement in pathElements)
            {
                var childNode = currentParent.Children.FirstOrDefault(x =>
                    x.Value.Equals(pathElement, StringComparison.OrdinalIgnoreCase));

                if (childNode is null)
                {
                    childNode = new FileSystemNode(pathElement, currentParent);
                    currentParent.Children.Add(childNode);
                }

                currentParent = childNode;
            }
        }

        public IEnumerable<string> GetFilesBySubString(string subString)
        {
            return null;
        }

        private string GetFullPath(FileSystemNode node)
        {
            var nameList = new List<string>();
            var currentNode = node;

            while (currentNode.Parent is not null)
            {
                nameList.Add(currentNode.Value);
                currentNode = currentNode.Parent;
            }

            nameList.Reverse();

            return string.Join(Path.DirectorySeparatorChar, nameList);
        }
    }
}
