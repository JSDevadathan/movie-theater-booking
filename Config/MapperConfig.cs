using AutoMapper;
using MovieTheaterBooking.DTO;
using MovieTheaterBooking.Models;

namespace MovieTheaterBooking.Config
{
    public class MapperConfig :Profile
    {
        public MapperConfig()
        {
            CreateMap<Show, ShowDto>();
            CreateMap<Show, ShowOccupancyDto>()
                .ForMember(dest => dest.BookedSeats,
                    opt => opt.MapFrom(src => src.TotalSeats - src.AvailableSeats))
                .ForMember(dest => dest.OccupancyPercentage,
                    opt => opt.MapFrom(src => ((double)(src.TotalSeats - src.AvailableSeats) / src.TotalSeats) * 100));
        }
    }
}
