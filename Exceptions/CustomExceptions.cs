namespace MovieTheaterBooking.Exceptions
{
    public class CustomExceptions
    {
        public class ShowNotFoundException : Exception
        {
            public int ShowId { get; }

            public ShowNotFoundException(int showId)
                : base($"Show with ID {showId} was not found.")
            {
                ShowId = showId;
            }

            public ShowNotFoundException(string message) : base(message){}
        }

        public class InvalidSeatsRequestedException(int requestedSeats, int availableSeats) : Exception($"Invalid number of seats requested. Requested: {requestedSeats}, Available: {availableSeats}")
        {
            //public int RequestedSeats { get; } = requestedSeats;
            //public int AvailableSeats { get; } = availableSeats;
        }

        public class InvalidEmailException(string email) : Exception($"Invalid email format: {email}")
        {
            //public string Email { get; } = email;
        }

        public class BookingFailedException(int showId, string customerEmail, Exception innerException) : Exception($"Failed to create booking for show {showId} and customer {customerEmail}", innerException)
        {
            //public int ShowId { get; } = showId;
            //public string CustomerEmail { get; } = customerEmail;
        }
        public class BookingNotFoundException(int bookingId) : Exception($"Booking with ID {bookingId} was not found.")
        {
            //public int BookingId { get; } = bookingId;
        }
        public class LateCancellationException(int bookingId, DateTime showTime, DateTime cancellationDeadline) : Exception($"Cannot cancel booking {bookingId}. Show starts at {showTime:g} and cancellation deadline was {cancellationDeadline:g}")
        {
            //public int BookingId { get; } = bookingId;
            //public DateTime ShowTime { get; } = showTime;
            //public DateTime CancellationDeadline { get; } = cancellationDeadline;
        }

        public class CancellationFailedException(int bookingId, Exception innerException) : Exception($"Failed to cancel booking {bookingId}", innerException)
        {
            //public int BookingId { get; } = bookingId;
        }
    }
}
