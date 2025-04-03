namespace Hotel.Models
{
    public class Room_bookings
    {
        public int Id { get; set; }
        public int RoomId { get; set; } // Foreign key property for Room
        public Room Room { get; set; } // Navigation property for Room
        public List<int> UserIds { get; set; } = new List<int>(); // Foreign key properties for Users
        public List<User> Users { get; set; } = new List<User>(); // Navigation properties for Users
        public DateTime Check_in { get; set; }
        public DateTime Check_out { get; set; }
    }
}