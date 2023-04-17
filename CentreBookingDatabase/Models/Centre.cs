namespace CentreBookingDatabase.Models
{
    public class Centre
    {
        public string? CentreName { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
