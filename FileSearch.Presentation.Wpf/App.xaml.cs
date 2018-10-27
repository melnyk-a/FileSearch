using FileSearch.Presentation.Wpf.Views;
using Ninject;
using Ninject.Extensions.Conventions;
using System.Windows;

namespace FileSearch.Presentation.Wpf
{
    public partial class App : Application
    {
        private StandardKernel CreateContainer()
        {
            var container = new StandardKernel();

            container.Bind(
                configurator => configurator
                .From("FileSearch.Presentation.Wpf", "FileSearch.Domain")
                .SelectAllClasses()
                .BindAllInterfaces()
                );

            container.Bind<ISynchronizationContext>().To<WpfSynchronizationContext>();

            return container;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = CreateContainer();
            MainWindow mainView = container.Get<MainWindow>();
            mainView.Show();
        }
    }
}