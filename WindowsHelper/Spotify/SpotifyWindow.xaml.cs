using System;
using System.Windows;
using WindowsHelper.Interfaces;
using WindowsHelper.Resources.UserControls;

namespace WindowsHelper.Spotify
{
    /// <summary>
    /// Interaktionslogik für SpotifyWindow.xaml
    /// </summary>
    public partial class SpotifyWindow : WindowsHelperWindow
    {
        public SpotifyWindow(IWindowSettings settings, SizeChangedEventHandler onSizeChanged, EventHandler onLocationChanged)
        : base(settings, onSizeChanged, onLocationChanged)
        {
            InitializeComponent();
        }
    }
}
