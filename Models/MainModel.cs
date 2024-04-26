using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SearchApp.Models
{
    [Serializable]
    public class MainModel : INotifyPropertyChanged
    {
        public MainModel() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ObservableCollection<DirectoryNode>? RootDirectories { get; set; }

        private string _startDir = string.Empty;
        public string StartDir
        {
            get => _startDir;
            set
            {
                if (_startDir != value)
                {
                    _startDir = value;
                    OnPropertyChanged(nameof(StartDir));
                }
            }
        }

        private string _templateName = string.Empty;
        public string TemplateName
        {
            get => _templateName;
            set
            {
                if (_templateName != value)
                {
                    _templateName = value;
                    OnPropertyChanged(nameof(TemplateName));
                }
            }
        }

        private string _curDir = string.Empty;
        public string CurDir
        {
            get => _curDir;
            set
            {
                if (_curDir != value)
                {
                    _curDir = value;
                    OnPropertyChanged(nameof(CurDir));
                }
            }
        }

        private int _allFilesCount = 0;
        public int AllFilesCount
        {
            get => _allFilesCount;
            set
            {
                if (_allFilesCount != value)
                {
                    _allFilesCount = value;
                    OnPropertyChanged(nameof(AllFilesCount));
                }
            }
        }

        private int _filesFindCount;
        public int FilesFindCount
        {
            get => _filesFindCount;
            set
            {
                if (_filesFindCount != value)
                {
                    _filesFindCount = value;
                    OnPropertyChanged(nameof(FilesFindCount));
                }
            }
        }

        private string _timePassed = "00:00:00";
        public string TimePassed
        {
            get => _timePassed;
            set
            {
                if (_timePassed != value)
                {
                    _timePassed = value;
                    OnPropertyChanged(nameof(TimePassed));
                }
            }
        }

        private bool _progressBarIsIndeterminate = false;
        public bool ProgressBarIsIndeterminate
        {
            get => _progressBarIsIndeterminate;
            set
            {
                if (_progressBarIsIndeterminate != value)
                {
                    _progressBarIsIndeterminate = value;
                    OnPropertyChanged(nameof(ProgressBarIsIndeterminate));
                }
            }
        }

        private bool _searchButtonIsEnable = true;
        public bool SearchButtonIsEnable
        {
            get => _searchButtonIsEnable;
            set
            {
                if (_searchButtonIsEnable != value)
                {
                    _searchButtonIsEnable = value;
                    OnPropertyChanged(nameof(SearchButtonIsEnable));
                }
            }
        }
    }
}
