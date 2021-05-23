using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PagedResponse<T>
    {
        [JsonPropertyName("first_page")]
        public string FirstPage { get; set; }

        [JsonPropertyName("last_page")]
        public string LastPage { get; set; }
        
        [JsonPropertyName("prev_page")]
        public string PrevPage { get; set; }
        
        [JsonPropertyName("next_page")]
        public string NextPage { get; set; }
        
        [JsonPropertyName("pages")]
        public int Pages { get; set; }
        
        [JsonPropertyName("total")]
        public int TotalCount { get; set; }
        
        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; }
    }
}
