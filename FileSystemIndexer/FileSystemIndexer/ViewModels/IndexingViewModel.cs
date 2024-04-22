using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FileSystemIndexer.Helpers;
using FileSystemIndexer.Models;

namespace FileSystemIndexer.ViewModels
{
    public class IndexingViewModel : ObservableObjectBase
    {
        private int _indexingFilesCount;
        private readonly FileSystemTree _fileSystemTree;

        public IndexingViewModel(FileSystemTree tree)
        {
            InitDrives();

            _fileSystemTree = tree;
            StartIndexingCommand = new RelayCommand(StartIndexingCommandExecute, () => 3 > 2);
            StopIndexingCommand = new AsyncRelayCommand(StopIndexingCommandExecute, () => 1 > 2);
            ContinueIndexingCommand = new AsyncRelayCommand(ContinueIndexingCommandExecute, () => 1 > 2);
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

        private void StartIndexingCommandExecute()
        {
            Task.Run(GetFiles);
        }

        private Task StopIndexingCommandExecute()
        {
            return null;
        }

        private Task ContinueIndexingCommandExecute()
        {
            return null;
        }

        private void GetFiles()
        {
            var selectedDrives = DrivesCollection.Where(x => x.IsChecked).ToList();

            if (selectedDrives.Count == 0)
            {
                return;
            }
            
            var allElements = FileSystemIterator.Iterate(selectedDrives[0].DriveName);

            foreach (var el in allElements)
            {
                _fileSystemTree.AddFile(el);
            }
        }
    }
}
