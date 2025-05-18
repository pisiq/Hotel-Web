// Data/Booking/BookingRepository.cs
using Hotel.Models;
using Hotel.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelContext _context;

        public BookingRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r.Location)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .ThenInclude(r => r.Location)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsForRoomAsync(int roomId)
        {
            var today = DateTime.Today;
            return await _context.Bookings
                .Include(b => b.User)
                .Where(b => b.RoomId == roomId &&
                            b.Status == BookingStatus.Confirmed &&
                            b.CheckOutDate >= today)
                .OrderBy(b => b.CheckInDate)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r.Location)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            // Get the room's availability
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            // Get count of active bookings for this room in the requested period
            var overlappingBookingsCount = await _context.Bookings
                .Where(b => b.RoomId == roomId &&
                            b.Status == BookingStatus.Confirmed &&
                            (b.CheckInDate <= checkIn && b.CheckOutDate > checkIn ||
                             b.CheckInDate < checkOut && b.CheckOutDate >= checkOut ||
                             b.CheckInDate >= checkIn && b.CheckOutDate <= checkOut))
                .CountAsync();

            // Room is available if overlapping bookings are less than total room count
            return overlappingBookingsCount < room.RoomCount;
        }

        public async Task AddAsync(Booking booking)
        {
            try
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Booking saved to database with ID: {booking.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BookingRepository.AddAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw; // Rethrow to be handled by service
            }
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}
