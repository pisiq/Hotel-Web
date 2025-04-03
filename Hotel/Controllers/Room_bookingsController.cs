using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Models;
using Hotel.Models.Context;

namespace Hotel.Controllers
{
    public class Room_bookingsController : Controller
    {
        private readonly HotelContext _context;

        public Room_bookingsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Room_bookings
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Room_Bookings.Include(r => r.Room);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Room_bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room_bookings = await _context.Room_Bookings
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room_bookings == null)
            {
                return NotFound();
            }

            return View(room_bookings);
        }

        // GET: Room_bookings/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id");
            return View();
        }

        // POST: Room_bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomId,UserIds,Check_in,Check_out")] Room_bookings room_bookings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room_bookings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", room_bookings.RoomId);
            return View(room_bookings);
        }

        // GET: Room_bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room_bookings = await _context.Room_Bookings.FindAsync(id);
            if (room_bookings == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", room_bookings.RoomId);
            return View(room_bookings);
        }

        // POST: Room_bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,UserIds,Check_in,Check_out")] Room_bookings room_bookings)
        {
            if (id != room_bookings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room_bookings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Room_bookingsExists(room_bookings.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", room_bookings.RoomId);
            return View(room_bookings);
        }

        // GET: Room_bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room_bookings = await _context.Room_Bookings
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room_bookings == null)
            {
                return NotFound();
            }

            return View(room_bookings);
        }

        // POST: Room_bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room_bookings = await _context.Room_Bookings.FindAsync(id);
            if (room_bookings != null)
            {
                _context.Room_Bookings.Remove(room_bookings);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Room_bookingsExists(int id)
        {
            return _context.Room_Bookings.Any(e => e.Id == id);
        }
    }
}
