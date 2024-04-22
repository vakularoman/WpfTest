namespace FileSystemIndexer.Helpers
{
    public class FileSystemNode
    {
        public FileSystemNode(string value, FileSystemNode? parent)
        {
            Parent = parent;
            Value = value;
            Children = [];
        }

        public FileSystemNode? Parent { get; set; }

        public List<FileSystemNode> Children { get; set; }

        public string Value { get; set; }
    }
}
