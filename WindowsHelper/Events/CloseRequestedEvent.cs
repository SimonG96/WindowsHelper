using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace WindowsHelper.Events
{
    public static class CloseRequestedEvent
    {
        public static event EventHandler CloseRequested;

        public static void RaiseCloseRequestedEvent(object sender)
        {
            CloseRequested?.Invoke(sender, null);
        }

        public static ICommand GetCloseRequestedCommand(object sender)
        {
            return new RelayCommand(() => RaiseCloseRequestedEvent(sender));
        }
    }
}