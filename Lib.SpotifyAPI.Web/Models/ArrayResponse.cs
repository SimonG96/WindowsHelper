using System.Collections.Generic;

namespace Lib.SpotifyAPI.Web.Models
{
    public class ListResponse<T> : BasicModel
    {
        public List<T> List { get; set; }
    }
}