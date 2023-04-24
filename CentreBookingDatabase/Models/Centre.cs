using System.ComponentModel.DataAnnotations;

namespace CentreBookingDatabase.Models
{
    public class Centre
    {
        [Key]
        public string? CentreName { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
