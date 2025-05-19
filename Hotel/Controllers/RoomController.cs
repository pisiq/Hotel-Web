using Hotel.Models;
using Hotel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly ILocationService _locationService;

        public RoomController(IRoomService roomService, ILocationService locationService)
        {
            _roomService = roomService;
            _locationService = locationService;
        }

        // GET: Room
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetAllRoomsWithDetailsAsync();
            return View(rooms);
        }

        // GET: Room/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var room = await _roomService.GetRoomByIdWithDetailsAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Room/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await PopulateLocationDropdown();
            return View(new RoomViewModel());
        }

        // POST: Room/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var room = viewModel.ToEntity();
                    await _roomService.AddRoomAsync(room, viewModel.MainPhoto, viewModel.AdditionalPhotos);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log exception
                    ModelState.AddModelError("", "Failed to create room: " + ex.Message);
                }
            }

            await PopulateLocationDropdown();
            return View(viewModel);
        }


        // GET: Room/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _roomService.GetRoomByIdWithDetailsAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            await PopulateLocationDropdown();
            return View(room);
        }

        // POST: Room/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Room room, IFormFile? mainPhoto, List<IFormFile>? additionalPhotos)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var success = await _roomService.UpdateRoomAsync(room, mainPhoto, additionalPhotos);
                if (!success)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateLocationDropdown();
            return View(room);
        }

        // GET: Room/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomService.GetRoomByIdWithDetailsAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _roomService.DeleteRoomAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Room deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete room. It may be in use by bookings.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting room: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Room/DeletePhoto
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePhoto(int id, int photoId)
        {
            await _roomService.DeleteRoomPhotoAsync(photoId);
            return RedirectToAction(nameof(Edit), new { id });
        }

        private async Task PopulateLocationDropdown()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            ViewBag.Locations = new SelectList(locations, "Id", "Name");
        }

        public async Task<IActionResult> AllRooms()
        {
            var rooms = await _roomService.GetAllRoomsWithDetailsAsync();
            return View(rooms);
        }
        
    }
}
