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
    public class User_BookingController : Controller
    {
        private readonly HotelContext _context;

        public User_BookingController(HotelContext context)
        {
            _context = context;
        }

        // GET: User_Booking
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.User_Bookings.Include(u => u.Room).Include(u => u.User);
            return View(await hotelContext.ToListAsync());
        }

        // GET: User_Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user_Booking = await _context.User_Bookings
                .Include(u => u.Room)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user_Booking == null)
            {
                return NotFound();
            }

            return View(user_Booking);
        }

        // GET: User_Booking/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: User_Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,RoomId,Check_in,Check_out")] User_Booking user_Booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user_Booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", user_Booking.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", user_Booking.UserId);
            return View(user_Booking);
        }

        // GET: User_Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user_Booking = await _context.User_Bookings.FindAsync(id);
            if (user_Booking == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", user_Booking.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", user_Booking.UserId);
            return View(user_Booking);
        }

        // POST: User_Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,RoomId,Check_in,Check_out")] User_Booking user_Booking)
        {
            if (id != user_Booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user_Booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!User_BookingExists(user_Booking.Id))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", user_Booking.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", user_Booking.UserId);
            return View(user_Booking);
        }

        // GET: User_Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user_Booking = await _context.User_Bookings
                .Include(u => u.Room)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user_Booking == null)
            {
                return NotFound();
            }

            return View(user_Booking);
        }

        // POST: User_Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user_Booking = await _context.User_Bookings.FindAsync(id);
            if (user_Booking != null)
            {
                _context.User_Bookings.Remove(user_Booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool User_BookingExists(int id)
        {
            return _context.User_Bookings.Any(e => e.Id == id);
        }
    }
}
