using System;
using System.Windows;
using WindowsHelper.Interfaces;
using WindowsHelper.Settings;

namespace WindowsHelper.Spotify
{
    public class SpotifySettings : IWindowSettings
    {
        public SpotifySettings(object parent)
        {
            Parent = parent;
        }

        public string Name => "Spotify";
        public object Parent { get; }


        [SettingsProperty(true, false)]
        public double WindowHeight { get; set; } = 450;

        [SettingsProperty(true, false)]
        public double WindowWidth { get; set; } = 800;

        [SettingsProperty(true, false)]
        public double WindowTop { get; set; } = 50;

        [SettingsProperty(true, false)]
        public double WindowLeft { get; set; } = 50;


        public void OnSpotifyWindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (!(sender is SettingsWindow settingsWindow))
                return;

            if (settingsWindow.WindowState != WindowState.Normal)
                return;

            WindowHeight = args.NewSize.Height;
            WindowWidth = args.NewSize.Width;
        }

        public void OnSpotifyWindowLocationChanged(object sender, EventArgs args)
        {
            if (!(sender is SettingsWindow settingsWindow))
                return;

            if (settingsWindow.WindowState != WindowState.Normal)
                return;

            WindowTop = settingsWindow.Top;
            WindowLeft = settingsWindow.Left;
        }
    }
}