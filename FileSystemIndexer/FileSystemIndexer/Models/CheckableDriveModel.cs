using FileSystemIndexer.ViewModels;

namespace FileSystemIndexer.Models
{
    public class CheckableDriveModel : ObservableObjectBase
    {
        private bool _isChecked;
        private string _driveName;
        private int _indexedCount;

        public CheckableDriveModel(string driveName)
        {
            DriveName = driveName;
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (value == _isChecked)
                {
                    return;
                }

                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public string DriveName
        {
            get => _driveName;
            set
            {
                if (value == _driveName)
                {
                    return;
                }

                _driveName = value;
                OnPropertyChanged();
            }
        }

        public int IndexedCount
        {
            get => _indexedCount;
            set
            {
                if (value == _indexedCount)
                {
                    return;
                }

                _indexedCount = value;
                OnPropertyChanged();
            }
        }
    }
}
