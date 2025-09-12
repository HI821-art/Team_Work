using HotelDb.Data;
using HotelDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApp.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelDbContext _context;

        public RoomsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Rooms - початкова сторінка
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms.Include(r => r.Images).ToListAsync();
            return View(rooms);
        }

        // GET: Rooms/Filter - повертає відфільтровані кімнати як PartialView
        [HttpGet]
        public async Task<IActionResult> Filter(double? minPrice, double? maxPrice, int? capacity, string type)
        {
            var roomsQuery = _context.Rooms.Include(r => r.Images).AsQueryable();

            if (minPrice.HasValue)
                roomsQuery = roomsQuery.Where(r => r.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                roomsQuery = roomsQuery.Where(r => r.Price <= maxPrice.Value);
            if (capacity.HasValue)
                roomsQuery = roomsQuery.Where(r => r.Capacity >= capacity.Value);
            if (!string.IsNullOrEmpty(type))
                roomsQuery = roomsQuery.Where(r => r.Type == type);

            var rooms = await roomsQuery.ToListAsync();

            return PartialView("_RoomCards", rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null) return NotFound();

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create() => View();

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.RoomId == id);
            if (room == null) return NotFound();

            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room room)
        {
            if (id != room.RoomId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null) return NotFound();

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomId == id);
        }
    }
}
