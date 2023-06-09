﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentreBookingDatabase.Models
{
    public class Booking
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? GuestName { get; set; }

        [Key]
        public string? CentreName { get; set; }
        // [ForeignKey("CentreName")]
        public Centre? Centre { get; set; }
    }
}
