using MovieTheaterBooking.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace MovieTheaterBooking.Data
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Show> Shows { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }
    }
}
