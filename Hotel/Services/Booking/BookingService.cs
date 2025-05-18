// Services/Booking/BookingService.cs
using Hotel.Data;
using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotel.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _bookingRepository.GetBookingsByUserIdAsync(userId);
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            if (checkIn >= checkOut)
            {
                throw new ArgumentException("Check-out date must be after check-in date");
            }

            return await _bookingRepository.IsRoomAvailableAsync(roomId, checkIn, checkOut);
        }

        public async Task<int> CreateBookingAsync(string userId, int roomId, DateTime checkIn, DateTime checkOut)
        {
            // Validate dates
            if (checkIn >= checkOut)
            {
                throw new ArgumentException("Check-out date must be after check-in date");
            }

            // Check if room exists
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new InvalidOperationException("Room not found");
            }

            // Check if room is available for these dates
            var isAvailable = await _bookingRepository.IsRoomAvailableAsync(
                roomId, checkIn, checkOut);

            if (!isAvailable)
            {
                throw new InvalidOperationException("The room is not available for the selected dates");
            }

            // Create booking
            var booking = new Booking
            {
                UserId = userId,
                RoomId = roomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                BookingDate = DateTime.Now,
                Status = BookingStatus.Confirmed
            };

            await _bookingRepository.AddAsync(booking);

            return booking.Id;
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            try
            {
                await _bookingRepository.UpdateAsync(booking);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelBookingAsync(int id)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(id);
                if (booking == null)
                {
                    return false;
                }

                booking.Status = BookingStatus.Cancelled;
                await _bookingRepository.UpdateAsync(booking);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
