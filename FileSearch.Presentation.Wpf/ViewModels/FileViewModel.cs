using FileSearch.Presentation.Wpf.Models;

namespace FileSearch.Presentation.Wpf.ViewModels
{
    internal sealed class FileViewModel
    {
        private string name;
        private string path;

        public FileViewModel(FileEntry fileEntry)
        {
            name = fileEntry.Name;
            path = fileEntry.Path;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Path
        {
            get => path;
            set => path = value;
        }
    }
}