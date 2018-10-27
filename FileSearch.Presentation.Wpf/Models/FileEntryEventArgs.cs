using System;

namespace FileSearch.Presentation.Wpf.Models
{
    internal class FileEntryEventArgs : EventArgs
    {
        private readonly FileEntry fileEntry;

        public FileEntryEventArgs(FileEntry fileEntry)
        {
            this.fileEntry = fileEntry;
        }

        public FileEntry FileEntry => fileEntry;
    }
}