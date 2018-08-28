using Newtonsoft.Json;

namespace Lib.SpotifyAPI.Web.Models
{
    public class FeaturedPlaylists : BasicModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("playlists")]
        public Paging<SimplePlaylist> Playlists { get; set; }
    }
}