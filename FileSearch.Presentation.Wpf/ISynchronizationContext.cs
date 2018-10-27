using System;

namespace FileSearch.Presentation.Wpf
{
    internal interface ISynchronizationContext
    {
        void Invoke(Action action);
    }
}