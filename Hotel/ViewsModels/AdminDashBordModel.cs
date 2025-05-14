using Hotel.Models;
using System.Collections.Generic;

namespace Hotel.ViewsModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public IEnumerable<User> RecentUsers { get; set; }

        // Add additional dashboard statistics as needed
        // public int TotalBookings { get; set; }
        // public int TotalRooms { get; set; }
        // public double OccupancyRate { get; set; }
    }
}