using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace WindowsHelper.Events
{
    public static class ShowSettingsWindowEvent
    {
        public static event EventHandler ShowSettingsWindow;

        public static void RaiseShowSettingsWindowEvent(object sender)
        {
            ShowSettingsWindow?.Invoke(sender, null);
        }

        public static ICommand GetShowSettingsWindowCommand(object sender)
        {
            return new RelayCommand(() => RaiseShowSettingsWindowEvent(sender));
        }
    }
}