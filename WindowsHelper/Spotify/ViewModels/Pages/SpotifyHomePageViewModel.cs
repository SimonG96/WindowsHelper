using WindowsHelper.Events;
using WindowsHelper.Spotify.Interfaces;
using GalaSoft.MvvmLight;

namespace WindowsHelper.Spotify.ViewModels.Pages
{
    public class SpotifyHomePageViewModel : ViewModelBase, ISpotifyPage
    {
        private bool _isSelected;

        public SpotifyHomePageViewModel()
        {

        }

        public string Name => "Home";

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);

                if (value)
                    SpotifyPageSelectedEvent.RaiseSpotifyPageSelectedEvent(this);
            }
        }
    }
}