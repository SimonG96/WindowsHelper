using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WindowsHelper.Events;
using WindowsHelper.Spotify.Interfaces;
using WindowsHelper.Spotify.ViewModels.Pages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Lib.Tools;

namespace WindowsHelper.Spotify.ViewModels
{
    public class SpotifyWindowViewModel : ViewModelBase
    {
        private ISpotifyPage _selectedPage;
        private bool _isPlaying;

        public SpotifyWindowViewModel()
        {
            if (IsInDesignMode)
            {
                Library = new ObservableCollection<ISpotifyPage>()
                {
                    new SpotifyAlbumPageViewModel(),
                    new SpotifyArtistPageViewModel(){IsSelected = true},
                    new SpotifySongsPageViewModel(),
                    new SpotifyRecentlyPlayedPageViewModel()
                };

                SelectedPage = Library.FirstOrDefault(p => p is SpotifyHomePageViewModel);
                return;
            }

            Library = new ObservableCollection<ISpotifyPage>()
            {
                new SpotifyHomePageViewModel(),
                new SpotifyAlbumPageViewModel(),
                new SpotifyArtistPageViewModel(),
                new SpotifySongsPageViewModel(),
                new SpotifyRecentlyPlayedPageViewModel()
            };

            SelectedPage = Library.FirstOrDefault(p => p is SpotifyHomePageViewModel);

            Playlists = new ObservableCollection<IPlaylist>();
            Playlists.EnableCollectionSynchronization();

            Friends = new ObservableCollection<IUser>();
            Friends.EnableCollectionSynchronization();

            SpotifyPageSelectedEvent.SpotifyPageSelected += OnPageSelected;
        }

        public ObservableCollection<ISpotifyPage> Library { get; set; }
        public ObservableCollection<IPlaylist> Playlists { get; set; }
        public ObservableCollection<IUser> Friends { get; set; }

        public ISpotifyPage SelectedPage
        {
            get => _selectedPage;
            set
            {
                _selectedPage = value;

                if (!_selectedPage.IsSelected)
                    _selectedPage.IsSelected = true;

                RaisePropertyChanged(() => SelectedPage);
            }
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                RaisePropertyChanged(() => IsPlaying);
            }
        }


        #region Commands

        //TODO: Finish Commands
        public ICommand CreateNewPlaylistCommand => new RelayCommand(CreateNewPlaylist);
        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand SkipNextCommand { get; }
        public ICommand SkipPreviousCommand { get; }
        

        #endregion


        #region Methods

        private void OnPageSelected(object sender, EventArgs args)
        {
            foreach (var page in Library.Where(p => p.IsSelected))
            {
                if (page.Equals(sender))
                {
                    SelectedPage = page;
                    continue;
                }

                page.IsSelected = false;
            }
        }

        private void CreateNewPlaylist()
        {

        }

        #endregion Methods
    }
}