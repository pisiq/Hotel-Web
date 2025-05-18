// ViewsModels/ProfileModel.cs
using Hotel.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewsModels
{
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Current profile picture path
        public string ProfilePicturePath { get; set; }

        // Property for uploading a new profile picture
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }

        // User's bookings
        public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();

        // Helper properties to categorize bookings
        public IEnumerable<Booking> ActiveBookings =>
            Bookings.Where(b => b.Status == BookingStatus.Confirmed && b.CheckOutDate >= DateTime.Today)
                   .OrderBy(b => b.CheckInDate);

        public IEnumerable<Booking> PastBookings =>
            Bookings.Where(b => b.Status == BookingStatus.Confirmed && b.CheckOutDate < DateTime.Today)
                   .OrderByDescending(b => b.CheckOutDate);

        public IEnumerable<Booking> CancelledBookings =>
            Bookings.Where(b => b.Status == BookingStatus.Cancelled)
                   .OrderByDescending(b => b.BookingDate);
    }
}