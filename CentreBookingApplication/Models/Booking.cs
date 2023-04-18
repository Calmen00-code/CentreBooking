namespace CentreBookingApplication.Models
{
    public class Booking
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? GuestName { get; set; }
        public string? CentreName { get; set; }
    }
}
