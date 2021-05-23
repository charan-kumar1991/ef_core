using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Ticket : BaseEntity<int>
    {
        [Required]
        public Guid PNR { get; set; }

        [Required]
        public int SeatNumber { get; set; }

        [Required]
        [MaxLength(15)]
        public string Berth { get; set; }

        [Required]
        [MaxLength(200)]
        public string From { get; set; }

        [Required]
        [MaxLength(200)]
        public string To { get; set; }

        [Required]
        public DateTime TravelDate { get; set; }

        [Required]
        public double Fare { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Customer Passenger { get; set; }
    }
}
