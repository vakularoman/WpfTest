using FileSystemIndexer.Helpers;

namespace FileSystemIndexer.ViewModels
{
    public sealed class MainViewModel : ObservableObjectBase, IDisposable
    {
        private readonly FileSystemTree _fileSystemTree = new();

        public MainViewModel()
        {
            IndexingViewModel = new IndexingViewModel(_fileSystemTree);
            ResultListViewModel = new ResultListViewModel(_fileSystemTree);
        }

        public IndexingViewModel IndexingViewModel { get; }

        public ResultListViewModel ResultListViewModel { get; }

        public void Dispose()
        {
            IndexingViewModel.Dispose();
        }
    }
}
