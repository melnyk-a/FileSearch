using System;
using System.Windows;

namespace FileSearch.Presentation.Wpf
{
    internal sealed class WpfSynchronizationContext : ISynchronizationContext
    {
        public void Invoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}