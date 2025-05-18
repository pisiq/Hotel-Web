// ViewsModels/BookingViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewsModels
{
    public class BookingViewModel
    {
        [Required]
        public int RoomId { get; set; }

        // Display properties for the view
        public string RoomType { get; set; }
        public decimal RoomPrice { get; set; }

        [Required(ErrorMessage = "Check-in date is required")]
        [Display(Name = "Check-in Date")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Check-out date is required")]
        [Display(Name = "Check-out Date")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        // Helper properties
        public int NumberOfNights => (CheckOutDate - CheckInDate).Days;
        public decimal TotalPrice => RoomPrice * NumberOfNights;
        public string? LocationName { get; set; }
    }
}
