using FileSearch.Presentation.Wpf.ViewModels;
using System.Windows;

namespace FileSearch.Presentation.Wpf.Views
{
    internal partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
