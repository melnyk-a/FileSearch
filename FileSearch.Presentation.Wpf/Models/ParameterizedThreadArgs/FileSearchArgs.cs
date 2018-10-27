namespace FileSearch.Presentation.Wpf.Models.ParameterizedThreadArgs
{
    internal sealed class FileSearchArgs
    {
        private readonly string path;
        private readonly string searchPattern;

        public FileSearchArgs(string path, string searchPattern)
        {
            this.path = path;
            this.searchPattern = searchPattern;
        }

        public string Path => path;

        public string SearchPattern => searchPattern;
    }
}