namespace WindowsHelper.Spotify.Interfaces
{
    public interface ISpotifyPage
    {
        string Name { get; }
        bool IsSelected { get; set; }
    }
}