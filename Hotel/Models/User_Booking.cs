namespace Hotel.Models
{
    public class User_Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign key property for User
        public User User { get; set; } // Navigation property for User
        public int RoomId { get; set; } // Foreign key property for Room
        public Room Room { get; set; } // Navigation property for Room
        public DateTime Check_in { get; set; }
        public DateTime Check_out { get; set; }
    }
}