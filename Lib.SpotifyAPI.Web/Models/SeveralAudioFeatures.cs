using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.SpotifyAPI.Web.Models
{
    public class SeveralAudioFeatures : BasicModel
    {
         [JsonProperty("audio_features")]
         public List<AudioFeatures> AudioFeatures { get; set; }
    }
}