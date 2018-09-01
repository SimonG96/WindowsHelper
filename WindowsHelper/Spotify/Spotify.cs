using System;
using System.Linq;
using WindowsHelper.Events;
using WindowsHelper.Interfaces;
using WindowsHelper.Settings;
using WindowsHelper.Spotify.ViewModels;
using Lib.Tools;

namespace WindowsHelper.Spotify
{
    public class Spotify : IPlugin
    {
        private const string SETTING_KEY = "Setting.";

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
            LoadSettings();
            return true;
        }

        public void DeInit()
        {
            SaveSettings();
        }

        private void OnShowSpotifyWindow(object sender, EventArgs args)
        {
            if (!(Settings is SpotifySettings spotifySettings))
                return;

            SpotifyWindow spotifyWindow = new SpotifyWindow(spotifySettings, spotifySettings.OnSpotifyWindowSizeChanged, spotifySettings.OnSpotifyWindowLocationChanged);
            spotifyWindow.DataContext = new SpotifyWindowViewModel();
            //spotifyWindow.SizeChanged += spotifySettings.OnSpotifyWindowSizeChanged;
            //spotifyWindow.LocationChanged += spotifySettings.OnSpotifyWindowLocationChanged;
            spotifyWindow.Show();
        }

        private void SaveSettings()
        {
            var properties = Settings.GetType().GetProperties().Where(p => p.IsDefined(typeof(SettingsPropertyAttribute), false));
            foreach (var property in properties)
            {
                SettingsPropertyAttribute attribute = (SettingsPropertyAttribute)property.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(SettingsPropertyAttribute));
                if (attribute == null)
                    continue;

                if (!attribute.Save)
                    continue;

                RegistryHelper.Instance.SubKey(Name).Set($"{SETTING_KEY}{property.Name}", property.GetValue(Settings));
            }
        }

        private void LoadSettings()
        {
            var properties = Settings.GetType().GetProperties().Where(p => p.IsDefined(typeof(SettingsPropertyAttribute), false));
            foreach (var property in properties)
            {
                SettingsPropertyAttribute attribute = (SettingsPropertyAttribute)property.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(SettingsPropertyAttribute));
                if (attribute == null)
                    continue;

                if (!attribute.Save)
                    continue;

                string key = $"{SETTING_KEY}{property.Name}";
                if (!RegistryHelper.Instance.SubKey(Name).Exists(key))
                    continue;

                var value = RegistryHelper.Instance.SubKey(Name).GetObject(key, property.GetValue(Settings));
                property.SetValue(Settings, value);
            }
        }

        public void Dispose()
        {
            
        }

        #endregion Methods
    }
}