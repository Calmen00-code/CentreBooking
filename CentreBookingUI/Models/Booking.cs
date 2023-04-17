namespace CentreBookingUI.Models
{
    public class Booking
    {
        public string CentreName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string GuestName { get; set; }
    }
}
