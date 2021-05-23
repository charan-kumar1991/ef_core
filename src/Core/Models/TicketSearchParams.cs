using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public class TicketSearchParams
    {
        [JsonPropertyName("from")]
        public string? From { get; set; }

        [JsonPropertyName("to")]
        public string? To { get; set; }

        [JsonPropertyName("page")]
        [Required]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        [Required]
        public int Limit { get; set; }
    }
}
