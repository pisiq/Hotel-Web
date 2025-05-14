namespace Hotel.Models
{
    public class BookingOfUser
    {
        public int Id { get; set; }

        // Change from int to string to match Identity's ID type
        public string UserId { get; set; }

        public User User { get; set; } // Navigation property for User
        public int RoomId { get; set; } // Foreign key property for Room
        public Room Room { get; set; } // Navigation property for Room
        public DateTime Check_in { get; set; }
        public DateTime Check_out { get; set; }
    }
}
