namespace MovieTheaterBooking.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public DateTime ShowTime { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal TicketPrice { get; set; }

    }
}
