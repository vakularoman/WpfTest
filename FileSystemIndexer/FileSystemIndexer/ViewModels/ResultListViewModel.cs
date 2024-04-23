using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FileSystemIndexer.Helpers;
using FileSystemIndexer.Models;

namespace FileSystemIndexer.ViewModels
{
    public class ResultListViewModel : ObservableObjectBase
    {
        private readonly FileSystemTree _fileSystemTree;
        private string _searchString;
        private int _searchResultCount;
        private ObservableCollection<FileModel> _searchResults = new();

        public ResultListViewModel(FileSystemTree tree)
        {
            _fileSystemTree = tree;
            SearchCommand = new AsyncRelayCommand(SearchCommandExecuteAsync);
        }

        public ICommand SearchCommand { get; }

        public ObservableCollection<FileModel> SearchResults
        {
            get => _searchResults;
            set
            {
                if (Equals(value, _searchResults))
                {
                    return;
                }

                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public string SearchString
        {
            get => _searchString;
            set
            {
                if (value == _searchString)
                {
                    return;
                }

                _searchString = value;
                OnPropertyChanged();
            }
        }

        public int SearchResultCount
        {
            get => _searchResultCount;
            set
            {
                if (value == _searchResultCount)
                {
                    return;
                }

                _searchResultCount = value;
                OnPropertyChanged();
            }
        }

        private async Task SearchCommandExecuteAsync()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                return;
            }

            var result = await Task.Run(() => _fileSystemTree.GetFilesBySubString(SearchString).ToList()).ConfigureAwait(false);

            Application.Current.Dispatcher.Invoke(() =>
            {
                SearchResults = new ObservableCollection<FileModel>(result.Select(x => new FileModel(x)));
            });

            SearchResultCount = SearchResults.Count;
        }
    }
}
