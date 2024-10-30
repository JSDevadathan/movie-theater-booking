namespace MovieTheaterBooking.DTO
{
    public class ShowOccupancyDto
    {
        public int ShowId { get; set; }
        public string MovieName { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
        public int AvailableSeats { get; set; }
        public double OccupancyPercentage { get; set; }
    }
}
