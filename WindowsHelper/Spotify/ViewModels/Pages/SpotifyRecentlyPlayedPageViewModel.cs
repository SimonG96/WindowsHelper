using WindowsHelper.Events;
using WindowsHelper.Spotify.Interfaces;
using GalaSoft.MvvmLight;

namespace WindowsHelper.Spotify.ViewModels.Pages
{
    public class SpotifyRecentlyPlayedPageViewModel : ViewModelBase, ISpotifyPage
    {
        private bool _isSelected;

        public SpotifyRecentlyPlayedPageViewModel()
        {

        }

        public string Name => "Recently Played";

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