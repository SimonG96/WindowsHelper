using WindowsHelper.Events;
using WindowsHelper.Spotify.Interfaces;
using GalaSoft.MvvmLight;

namespace WindowsHelper.Spotify.ViewModels.Pages
{
    public class SpotifyArtistPageViewModel : ViewModelBase, ISpotifyPage
    {
        private bool _isSelected;

        public SpotifyArtistPageViewModel()
        {

        }


        public string Name => "Artists";

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