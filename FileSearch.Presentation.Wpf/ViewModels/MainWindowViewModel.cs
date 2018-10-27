using FileSearch.Presentation.Wpf.Models;
using FileSearch.Utilities.Wpf.Attributes;
using FileSearch.Utilities.Wpf.Commands;
using FileSearch.Utilities.Wpf.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileSearch.Presentation.Wpf.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModel
    {
        private readonly LocalDirectory localDirectory;
        private readonly Command pauseCommand;
        private readonly Command resumeCommand;
        private readonly Command searchCommand;
        private readonly ICollection<FileViewModel> searchResults = new ObservableCollection<FileViewModel>();
        private readonly Command stopCommand;
        private readonly ISynchronizationContext synchronizationContext;

        private bool searchInProcess = false;
        private bool searchIsPaused = false;
        private string searchPattern = string.Empty;
        private string selectedDirectory = string.Empty;

        public MainWindowViewModel(ISynchronizationContext synchronizationContext, LocalDirectory localDirectory)
        {
            this.synchronizationContext = synchronizationContext;
            this.localDirectory = localDirectory;

            localDirectory.FileFinded += (sender, e) =>
             {
                 synchronizationContext.Invoke(() =>
                 {
                     searchResults.Add(new FileViewModel(e.FileEntry));
                 });
             };

            localDirectory.SearchStarted += (sender, e) =>
              {
                  synchronizationContext.Invoke(() =>
                  {
                      SearchInProcess = true;
                  });

              };

            localDirectory.SearchFinished += (sender, e) =>
            {
                synchronizationContext.Invoke(() =>
                {
                    SearchInProcess = false;
                });
            };

            localDirectory.SearchPaused += (sender, e) =>
            {
                synchronizationContext.Invoke(() =>
                {
                    SearchIsPaused = true;
                });
            };

            localDirectory.SearchResumed += (sender, e) =>
            {
                synchronizationContext.Invoke(() =>
                {
                    SearchIsPaused = false;
                });
            };

            searchCommand = new DelegateCommand(Search, () => CanSearch);
            pauseCommand = new DelegateCommand(() => localDirectory.PauseSearch(), () => CanPause);
            resumeCommand = new DelegateCommand(() => localDirectory.ResumeSearch(), () => CanResume);
            stopCommand = new DelegateCommand(() => localDirectory.StopSearch(), () => CanStop);
        }

        [DependsUponProperty(nameof(SearchInProcess))]
        [DependsUponProperty(nameof(SearchIsPaused))]
        public bool CanPause => !SearchIsPaused && SearchInProcess;

        [DependsUponProperty(nameof(SearchInProcess))]
        [DependsUponProperty(nameof(SearchIsPaused))]
        public bool CanResume => SearchInProcess && SearchIsPaused;

        [DependsUponProperty(nameof(SelectedDirectory))]
        [DependsUponProperty(nameof(SearchPattern))]
        [DependsUponProperty(nameof(SearchInProcess))]
        public bool CanSearch => selectedDirectory != string.Empty && searchPattern != string.Empty && !SearchInProcess;

        [DependsUponProperty(nameof(CanSearch))]
        [DependsUponProperty(nameof(SearchInProcess))]
        public bool CanStop => SearchInProcess;

        public IEnumerable<string> LogicalDrives => localDirectory.GetLogicalDrives();

        [RaiseCanExecuteDependsUpon(nameof(CanPause))]
        public Command PauseCommand => pauseCommand;

        [RaiseCanExecuteDependsUpon(nameof(CanSearch))]
        public Command SearchCommand => searchCommand;

        [RaiseCanExecuteDependsUpon(nameof(CanResume))]
        public Command ResumeCommand => resumeCommand;

        public bool SearchInProcess
        {
            get => searchInProcess;
            set => SetProperty(ref searchInProcess, value);
        }

        public bool SearchIsPaused
        {
            get => searchIsPaused;
            set => SetProperty(ref searchIsPaused, value);
        }

        public string SearchPattern
        {
            get => searchPattern;
            set => SetProperty(ref searchPattern, value ?? string.Empty);
        }

        public IEnumerable<FileViewModel> SearchResults => searchResults;

        public string SelectedDirectory
        {
            get => selectedDirectory;
            set => SetProperty(ref selectedDirectory, value ?? string.Empty);
        }

        [RaiseCanExecuteDependsUpon(nameof(CanStop))]
        public Command StopCommand => stopCommand;

        private void Reset()
        {
            searchResults.Clear();
            SearchIsPaused = false;
            SearchInProcess = false;
        }

        private void Search()
        {
            Reset();
            localDirectory.StartSearch(selectedDirectory, searchPattern);
        }
    }
}