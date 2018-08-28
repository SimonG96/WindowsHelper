using WindowsHelper.Interfaces;

namespace WindowsHelper.Spotify
{
    public class SpotifySettings : ISettings
    {
        public SpotifySettings(object parent)
        {
            Parent = parent;
        }

        public string Name => "Spotify";
        public object Parent { get; }
    }
}