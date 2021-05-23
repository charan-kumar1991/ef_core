using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models.Requests
{
    public class AddTicketDto
    {
        [JsonPropertyName("seat_number")]
        [Required]
        public int SeatNumber { get; set; }

        [JsonPropertyName("berth")]
        [Required]
        [MaxLength(15)]
        public string Berth { get; set; }

        [JsonPropertyName("from")]
        [Required]
        [MaxLength(200)]
        public string From { get; set; }

        [JsonPropertyName("to")]
        [Required]
        [MaxLength(200)]
        public string To { get; set; }

        [JsonPropertyName("travel_date")]
        [Required]
        public DateTime TravelDate { get; set; }

        [JsonPropertyName("customer_id")]
        [Required]
        public int CustomerId { get; set; }
    }
}
