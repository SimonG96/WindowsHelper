using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace WindowsHelper.Events
{
    public static class ShowSpotifyWindowEvent
    {
        public static event EventHandler ShowSpotifyWindow;

        public static void RaiseShowSpotifyWindowEvent(object sender)
        {
            ShowSpotifyWindow?.Invoke(sender, null);
        }

        public static ICommand GetShowSpotifyWindowCommand(object sender)
        {
            return new RelayCommand(() => RaiseShowSpotifyWindowEvent(sender));
        }
    }
}