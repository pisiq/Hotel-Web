using Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Models.Context
{
    public class HotelContext : DbContext
    {
        // DbSets representing tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User_Booking> User_Bookings { get; set; }
        public DbSet<Room_bookings> Room_Bookings { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        public HotelContext(DbContextOptions options) : base(options)
        {

        }

    }
}