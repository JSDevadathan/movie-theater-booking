using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterBooking.Data;
using MovieTheaterBooking.DTO;
using static MovieTheaterBooking.Exceptions.CustomExceptions;

namespace MovieTheaterBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController(MovieDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly MovieDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public IActionResult GetShows([FromQuery] DateTime? date, [FromQuery] string movieName, [FromQuery] int? minSeats)
        {
            try
            {
                var shows = _context.Shows.AsQueryable();
                if (date.HasValue)
                {
                    var utcDate = date.Value.ToUniversalTime().Date;
                    shows = shows.Where(s => s.ShowTime.Date == utcDate);
                }
                if (!string.IsNullOrEmpty(movieName))
                {
                    var lowerMovieName = movieName.ToLower();
                    shows = shows.Where(s => s.MovieName.ToLower().Contains(lowerMovieName));
                }
                if (minSeats.HasValue)
                {
                    shows = shows.Where(s => s.AvailableSeats >= minSeats.Value);
                }
                var result = shows.ToList().Select(s => _mapper.Map<ShowDto>(s)).ToList();
                if (!result.Any())
                {
                    throw new ShowNotFoundException("Show not found");
                }
                return Ok(new { shows = result });
            }
            catch (ShowNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching shows." });
            }
        }



        [HttpGet("{id}/occupancy")]
        public IActionResult GetShowOccupancy(int id)
        {
            var show = _context.Shows.Find(id);
            if (show == null)
            {
                return NotFound();
            }
            var bookedSeats = show.TotalSeats - show.AvailableSeats;
            var occupancyPercentage = (double)bookedSeats / show.TotalSeats * 100;
            return Ok(new
            {
                showId = show.Id,
                movieName = show.MovieName,
                totalSeats = show.TotalSeats,
                bookedSeats,
                availableSeats = show.AvailableSeats,
                occupancyPercentage
            });
        }

    }
}
