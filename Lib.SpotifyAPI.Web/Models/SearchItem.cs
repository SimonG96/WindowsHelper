﻿using Newtonsoft.Json;

namespace Lib.SpotifyAPI.Web.Models
{
    public class SearchItem : BasicModel
    {
        [JsonProperty("artists")]
        public Paging<FullArtist> Artists { get; set; }

        [JsonProperty("albums")]
        public Paging<SimpleAlbum> Albums { get; set; }

        [JsonProperty("tracks")]
        public Paging<FullTrack> Tracks { get; set; }

        [JsonProperty("playlists")]
        public Paging<SimplePlaylist> Playlists { get; set; } 
    }
}