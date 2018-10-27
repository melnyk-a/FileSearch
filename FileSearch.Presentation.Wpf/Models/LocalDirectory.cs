using FileSearch.Presentation.Wpf.Models.ParameterizedThreadArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FileSearch.Presentation.Wpf.Models
{
    internal sealed class LocalDirectory
    {
        private Thread searchFileThread;

        public IEnumerable<string> GetLogicalDrives() => Directory.GetLogicalDrives();

        public event EventHandler<FileEntryEventArgs> FileFinded;
        public event EventHandler SearchFinished;
        public event EventHandler SearchPaused;
        public event EventHandler SearchResumed;
        public event EventHandler SearchStarted;

        public void OnFileFinded(FileEntryEventArgs eventArgs) => FileFinded?.Invoke(this, eventArgs);

        public void OnSearchFinished(EventArgs e) => SearchFinished?.Invoke(this, e);

        public void OnSearchPaused(EventArgs e) => SearchPaused?.Invoke(this, e);

        public void OnSearchResumed(EventArgs e) => SearchResumed?.Invoke(this, e);

        public void OnSearchStarted(EventArgs e) => SearchStarted?.Invoke(this, e);

        public void PauseSearch()
        {
            OnSearchPaused(EventArgs.Empty);
            searchFileThread.Suspend();
        }

        public void ResumeSearch()
        {
            OnSearchResumed(EventArgs.Empty);
            searchFileThread.Resume();
        }

        private void SearchFile(object obj)
        {
            OnSearchStarted(EventArgs.Empty);
            FileSearchArgs fileSearchArgs = (FileSearchArgs)obj;

            SearchInDirectory(fileSearchArgs.Path, fileSearchArgs.SearchPattern);
            SearchInSubDirectory(fileSearchArgs.Path, fileSearchArgs.SearchPattern);

            OnSearchFinished(EventArgs.Empty);
        }

        private void SearchInDirectory(string path, string searchPattern)
        {
            var files = Directory.GetFiles(path, searchPattern);
            if (files.Length != 0)
            {
                foreach (string file in files)
                {
                    OnFileFinded(new FileEntryEventArgs(new FileEntry(file)));
                }
            }
        }

        private void SearchInSubDirectory(string path, string searchPattern)
        {
            foreach (var subDirectory in Directory.GetDirectories(path))
            {
                try
                {
                    SearchInDirectory(subDirectory, searchPattern);
                    SearchInSubDirectory(subDirectory, searchPattern);
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        public void StartSearch(string path, string searchPattern)
        {
            searchFileThread = new Thread(SearchFile) { IsBackground = true };
            searchFileThread.Start(new FileSearchArgs(path, searchPattern));
        }

        public void StopSearch()
        {
            searchFileThread.Resume();
            searchFileThread.Abort();
            OnSearchFinished(EventArgs.Empty);
        }
    }
}