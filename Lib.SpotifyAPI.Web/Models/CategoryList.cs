using Newtonsoft.Json;

namespace Lib.SpotifyAPI.Web.Models
{
    public class CategoryList : BasicModel
    {
        [JsonProperty("categories")]
        public Paging<Category> Categories { get; set; }
    }
}