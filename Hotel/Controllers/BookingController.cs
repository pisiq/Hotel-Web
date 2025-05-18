// Controllers/BookingController.cs
using Hotel.Models;
using Hotel.Services;
using Hotel.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    [Authorize] // Require login for all booking actions
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;

        public BookingController(
            IBookingService bookingService,
            IRoomService roomService,
            IUserService userService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _userService = userService;
        }

        // GET: Booking/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Users");
            }

            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return View(bookings);
        }

        // GET: Booking/AllBookings - Admin only
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Check if user is authorized to view this booking
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (booking.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(booking);
        }

        // GET: Booking/Create/5 (roomId)
        public async Task<IActionResult> Create(int roomId)
        {
            var room = await _roomService.GetRoomByIdWithDetailsAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            // Only allow booking if rooms are available
            if (room.RoomCount <= 0)
            {
                TempData["ErrorMessage"] = "This room is currently not available for booking.";
                return RedirectToAction("AllRooms", "Room");
            }

            var viewModel = new BookingViewModel
            {
                RoomId = roomId,
                RoomType = room.Type,
                RoomPrice = room.Price,
                LocationName = room.Location?.Name,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(2)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel viewModel)
        {
            // Add debugging line to check if we're hitting this action
            Console.WriteLine($"Create action called with RoomId: {viewModel.RoomId}, Dates: {viewModel.CheckInDate} - {viewModel.CheckOutDate}");

            if (!ModelState.IsValid)
            {
                // Log validation errors
                Console.WriteLine("ModelState is invalid:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"{state.Key}: {error.ErrorMessage}");
                    }
                }
            }

            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("User ID is null or empty");
                    return RedirectToAction("Login", "Users");
                }

                Console.WriteLine($"User ID: {userId}");

                // Check if room is available
                var isAvailable = await _bookingService.IsRoomAvailableAsync(
                    viewModel.RoomId, viewModel.CheckInDate, viewModel.CheckOutDate);

                Console.WriteLine($"Room available: {isAvailable}");

                if (!isAvailable)
                {
                    ModelState.AddModelError("", "This room is not available for the selected dates.");
                    // Refresh room info
                    var room = await _roomService.GetRoomByIdWithDetailsAsync(viewModel.RoomId);
                    if (room != null)
                    {
                        viewModel.RoomType = room.Type;
                        viewModel.RoomPrice = room.Price;
                        viewModel.LocationName = room.Location?.Name;
                    }
                    return View(viewModel);
                }

                // Create the booking
                Console.WriteLine("Creating booking...");
                var bookingId = await _bookingService.CreateBookingAsync(
                    userId, viewModel.RoomId, viewModel.CheckInDate, viewModel.CheckOutDate);

                Console.WriteLine($"Booking created with ID: {bookingId}");
                TempData["SuccessMessage"] = "Your booking has been confirmed!";

                // Redirect to Home/Index instead of Booking/Details
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Exception creating booking: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                ModelState.AddModelError("", $"Error creating booking: {ex.Message}");
            }

            // Existing code for refresh...
            var roomData = await _roomService.GetRoomByIdWithDetailsAsync(viewModel.RoomId);
            if (roomData != null)
            {
                viewModel.RoomType = roomData.Type;
                viewModel.RoomPrice = roomData.Price;
                viewModel.LocationName = roomData.Location?.Name;
            }

            return View(viewModel);
        }



        // POST: Booking/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Check if user is authorized to cancel this booking
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (booking.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _bookingService.CancelBookingAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Your booking has been cancelled.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to cancel the booking.";
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(AllBookings));
            }

            return RedirectToAction(nameof(MyBookings));
        }
    }
}
