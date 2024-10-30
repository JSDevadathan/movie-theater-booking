namespace MovieTheaterBooking.DTO
{
    public class CreateBookingDto
    {
        public int ShowId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
