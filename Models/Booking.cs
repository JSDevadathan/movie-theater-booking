namespace MovieTheaterBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int ShowId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int NumberOfSeats { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
