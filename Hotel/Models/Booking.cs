using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class Booking
    {
        public int Id { get; set; }

        // User who made the booking
        public string UserId { get; set; }
        public User User { get; set; }

        // Room being booked
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Required]
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [Display(Name = "Check-out Date")]
        public DateTime CheckOutDate { get; set; }

        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Display(Name = "Booking Status")]
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    }

    public enum BookingStatus
    {
        Confirmed,
        Cancelled,
        Completed
    }
}