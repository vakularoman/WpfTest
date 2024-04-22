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
            return GetFilesBySubstringInternal(Root, subString);
        }

        private IEnumerable<string> GetFilesBySubstringInternal(FileSystemNode node, string subString)
        {
            if (node.Value.Contains(subString, StringComparison.OrdinalIgnoreCase))
            {
                foreach (var el in GetAllFileFromNode(node))
                {
                    yield return GetFullPath(el);
                }
            }

            foreach (var child in node.Children)
            {
                foreach (var el in GetFilesBySubstringInternal(child, subString))
                {
                    yield return el;
                }
            }
        }

        private IEnumerable<FileSystemNode> GetAllFileFromNode(FileSystemNode node)
        {
            if (node.Children.Count == 0)
            {
                yield return node;
            }
            else
            {
                foreach (var child in node.Children)
                {
                    foreach (var childResult in GetAllFileFromNode(child))
                    {
                        yield return childResult;
                    }
                }
            }
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
