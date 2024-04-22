﻿using FileSystemIndexer.Helpers;

namespace FileSystemIndexer.ViewModels
{
    public class MainViewModel : ObservableObjectBase
    {
        private readonly FileSystemTree _fileSystemTree = new();

        public MainViewModel()
        {
            IndexingViewModel = new IndexingViewModel(_fileSystemTree);
        }

        public IndexingViewModel IndexingViewModel { get; }
    }
}