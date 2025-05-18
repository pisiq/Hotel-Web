// Services/Booking/IBookingService.cs
using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotel.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
        Task<int> CreateBookingAsync(string userId, int roomId, DateTime checkIn, DateTime checkOut);
        Task<bool> UpdateBookingAsync(Booking booking);
        Task<bool> CancelBookingAsync(int id);
    }
}
