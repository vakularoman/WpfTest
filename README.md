# File System Indexer

This application is a simple file system indexer.
This application is implemented in C# WPF using MVVM approach.

The application contains two parts. Left part of window contains indexing logic with Start, Stop and Continue indexing,
also in this tab you can choose drives for indexing. Right part of window has search logic by substring in the path and 
shows some additional information as icon, is file hidden, is file readonly, file size. 

This is all for now, but in the future there are plans to add serialization to save state after exit and change the logic in favor of multithreading.