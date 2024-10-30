using Microsoft.AspNetCore.Mvc;
using MovieTheaterBooking.Data;
using MovieTheaterBooking.DTO;
using MovieTheaterBooking.Exceptions;
using MovieTheaterBooking.Models;

namespace MovieTheaterBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController(MovieDbContext context) : ControllerBase
    {

        private readonly MovieDbContext _context = context;

        [HttpPost]
        public async Task<IActionResult> BookTickets([FromBody] CreateBookingDto request)
        {
            try
            {
                var show = _context.Shows.Find(request.ShowId)
                           ?? throw new CustomExceptions.ShowNotFoundException(request.ShowId);

                if (request.NumberOfSeats <= 0 || request.NumberOfSeats > show.AvailableSeats)
                {
                    throw new CustomExceptions.InvalidSeatsRequestedException(request.NumberOfSeats, show.AvailableSeats);
                }

                if (!IsValidEmail(request.CustomerEmail))
                {
                    throw new CustomExceptions.InvalidEmailException(request.CustomerEmail);
                }

                show.AvailableSeats -= request.NumberOfSeats;
                _context.Shows.Update(show);

                var booking = new Booking
                {
                    ShowId = request.ShowId,
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail,
                    NumberOfSeats = request.NumberOfSeats,
                    BookingTime = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new CustomExceptions.BookingFailedException(request.ShowId, request.CustomerEmail, ex);
                }

                return Ok(booking);
            }
            catch (CustomExceptions.ShowNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CustomExceptions.InvalidSeatsRequestedException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (CustomExceptions.InvalidEmailException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (CustomExceptions.BookingFailedException ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your booking" });
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var booking = _context.Bookings.Find(id)
                              ?? throw new CustomExceptions.BookingNotFoundException(id);

                var show = _context.Shows.Find(booking.ShowId)
                           ?? throw new CustomExceptions.ShowNotFoundException(booking.ShowId);

                var cancellationDeadline = show.ShowTime.AddHours(-2);
                if (DateTime.UtcNow >= cancellationDeadline)
                {
                    throw new CustomExceptions.LateCancellationException(id, show.ShowTime, cancellationDeadline);
                }

                show.AvailableSeats += booking.NumberOfSeats;
                _context.Shows.Update(show);
                _context.Bookings.Remove(booking);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new CustomExceptions.CancellationFailedException(id, ex);
                }

                return NoContent();
            }
            catch (CustomExceptions.BookingNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CustomExceptions.ShowNotFoundException ex)
            {
                return StatusCode(500, new { message = "An internal error occurred" });
            }
            catch (CustomExceptions.LateCancellationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (CustomExceptions.CancellationFailedException ex)
            {
                return StatusCode(500, new { message = "An error occurred while canceling your booking" });
            }
        }
    }
}
