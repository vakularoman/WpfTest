using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FileSystemIndexer.Helpers;
using FileSystemIndexer.Models;

namespace FileSystemIndexer.ViewModels
{
    public sealed class IndexingViewModel : ObservableObjectBase, IDisposable
    {
        private int _indexingFilesCount;
        private readonly FileSystemTree _fileSystemTree;
        private bool _isIndexingRunning;
        private CancellationTokenSource _cancellationToken;
        private IEnumerable<string> _currentEnumerable;
        private List<string> _currentDrives;
        private bool _isIndexingInProgress;

        public IndexingViewModel(FileSystemTree tree)
        {
            InitDrives();

            _fileSystemTree = tree;
            StartIndexingCommand = new AsyncRelayCommand(StartIndexingCommandExecuteAsync);
            StopIndexingCommand = new AsyncRelayCommand(StopIndexingCommandExecuteAsync);
            ContinueIndexingCommand = new AsyncRelayCommand(ContinueIndexingCommandExecuteAsync);
        }

        public ObservableCollection<CheckableDriveModel> DrivesCollection { get; private set; }

        public ICommand StartIndexingCommand { get; }

        public ICommand StopIndexingCommand { get; }

        public ICommand ContinueIndexingCommand { get; }

        public int IndexingFilesCount
        {
            get => _indexingFilesCount;
            set
            {
                if (value == _indexingFilesCount)
                {
                    return;
                }

                _indexingFilesCount = value;
                OnPropertyChanged();
            }
        }

        public bool IsIndexingRunning
        {
            get => _isIndexingRunning;
            set
            {
                if (value == _isIndexingRunning)
                {
                    return;
                }

                _isIndexingRunning = value;
                OnPropertyChanged();
            }
        }

        public bool IsIndexingInProgress
        {
            get => _isIndexingInProgress;
            set
            {
                if (value == _isIndexingInProgress)
                {
                    return;
                }

                _isIndexingInProgress = value;
                OnPropertyChanged();
            }
        }

        private void InitDrives()
        {
            var drivesInfos = DriveInfo.GetDrives();
            var drivesCollection = new List<CheckableDriveModel>();

            foreach (var drive in drivesInfos)
            {
                var newEl = new CheckableDriveModel(drive.Name);
                drivesCollection.Add(newEl);
            }

            DrivesCollection = new ObservableCollection<CheckableDriveModel>(drivesCollection);
        }

        private async Task StartIndexingCommandExecuteAsync()
        {
            IsIndexingRunning = true;
            IsIndexingInProgress = true;
            IndexingFilesCount = 0;
            _fileSystemTree.Clear();

            try
            {
                await Task.Run(GetFiles).ConfigureAwait(false);
                IsIndexingInProgress = false;
            }
            catch (Exception)
            {
                // Ignore
            }
            finally
            {
                IsIndexingRunning = false;
            }
        }

        private async Task StopIndexingCommandExecuteAsync()
        {
            await _cancellationToken.CancelAsync().ConfigureAwait(false);
        }

        private async Task ContinueIndexingCommandExecuteAsync()
        {
            IsIndexingRunning = true;

            try
            {
                await Task.Run(GetFiles).ConfigureAwait(false);
                IsIndexingInProgress = false;
            }
            catch (Exception)
            {
                // Ignore
            }
            finally
            {
                IsIndexingRunning = false;
            }
        }

        private void GetFiles()
        {
            const int counterStep = 250;

            _cancellationToken?.Dispose();
            _cancellationToken = new CancellationTokenSource();
            var newToken = _cancellationToken.Token;

            var selectedDrives = DrivesCollection.Where(x => x.IsChecked).Select(x => x.DriveName).ToList();

            if (_currentDrives is null || !_currentDrives.SequenceEqual(selectedDrives))
            {
                IndexingFilesCount = 0;

                if (selectedDrives.Count == 0)
                {
                    return;
                }

                var iterationOrder = Enumerable.Empty<string>();

                foreach (var selectedDrive in selectedDrives)
                {
                    iterationOrder = iterationOrder.Concat(FileSystemIterator.Iterate(selectedDrive));
                }

                _currentDrives = selectedDrives;
                _currentEnumerable = iterationOrder;
            }

            var counter = IndexingFilesCount;

            foreach (var el in _currentEnumerable)
            {
                counter++;
                _fileSystemTree.AddFile(el);

                if (counter % counterStep == 0)
                {
                    IndexingFilesCount = counter;
                    newToken.ThrowIfCancellationRequested();
                }
            }

            IndexingFilesCount = counter;
        }

        public void Dispose()
        {
            _cancellationToken?.Dispose();
        }
    }
}
