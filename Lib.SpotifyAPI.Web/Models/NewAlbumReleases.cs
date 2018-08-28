using Newtonsoft.Json;

namespace Lib.SpotifyAPI.Web.Models
{
    public class NewAlbumReleases : BasicModel
    {
        [JsonProperty("albums")]
        public Paging<SimpleAlbum> Albums { get; set; }
    }
}