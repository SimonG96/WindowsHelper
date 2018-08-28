using WindowsHelper.Events;
using WindowsHelper.Spotify.Interfaces;
using GalaSoft.MvvmLight;

namespace WindowsHelper.Spotify.ViewModels.Pages
{
    public class SpotifyAlbumPageViewModel :  ViewModelBase, ISpotifyPage
    {
        private bool _isSelected;

        public SpotifyAlbumPageViewModel()
        {

        }

        public string Name => "Albums";

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