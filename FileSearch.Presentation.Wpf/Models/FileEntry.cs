namespace FileSearch.Presentation.Wpf.Models
{
    internal sealed class FileEntry
    {
        private readonly string name;
        private readonly string path;

        public FileEntry(string path)
        {
            this.path = path;
            name = System.IO.Path.GetFileName(path);
        }

        public string Name => name;

        public string Path => path;
    }
}