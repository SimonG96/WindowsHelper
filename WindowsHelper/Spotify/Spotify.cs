using System;
using WindowsHelper.Events;
using WindowsHelper.Interfaces;
using WindowsHelper.Spotify.ViewModels;

namespace WindowsHelper.Spotify
{
    public class Spotify : IPlugin
    {
        public Spotify()
        {
            Settings = new SpotifySettings(this);

            ShowSpotifyWindowEvent.ShowSpotifyWindow += OnShowSpotifyWindow;
        }

        public string Name => nameof(Spotify);
        public ISettings Settings { get; }


        #region Methods

        public bool Init()
        {
            return true;
        }

        public void DeInit()
        {
            
        }

        private void OnShowSpotifyWindow(object sender, EventArgs args)
        {
            SpotifyWindow spotifyWindow = new SpotifyWindow();
            spotifyWindow.DataContext = new SpotifyWindowViewModel();
            spotifyWindow.Show();
        }


        public void Dispose()
        {
            
        }

        #endregion Methods
    }
}