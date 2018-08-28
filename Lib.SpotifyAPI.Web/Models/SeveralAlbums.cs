using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.SpotifyAPI.Web.Models
{
    public class SeveralAlbums : BasicModel
    {
        [JsonProperty("albums")]
        public List<FullAlbum> Albums { get; set; }
    }
}