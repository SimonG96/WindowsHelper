using System.Windows.Input;
using WindowsHelper.Events;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace WindowsHelper.NotifyIcon
{
    public class NotifyIconViewModel : ViewModelBase
    {
        public ICommand ShowWindowCommand => new RelayCommand(ShowWindow);
        public ICommand ShowSettingsWindowCommand => new RelayCommand(ShowSettingsWindow);
        public ICommand ShowSpotifyWindowCommand => new RelayCommand(ShowSpotifyWindow);
        public ICommand ExitCommand => new RelayCommand(Exit);


        private void ShowWindow()
        {
            MainWindowEnabledEvent.RaiseMainWindowEnabledEvent(this, true);
        }

        private void ShowSettingsWindow()
        {
            ShowSettingsWindowEvent.RaiseShowSettingsWindowEvent(this);
        }

        private void ShowSpotifyWindow()
        {
            ShowSpotifyWindowEvent.RaiseShowSpotifyWindowEvent(this);
        }

        private void Exit()
        {
            CloseRequestedEvent.RaiseCloseRequestedEvent(this);
        }
    }
}