using HotelDb.Data;
using HotelDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelApp.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RoomsController(HotelDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms.Include(r => r.Images).ToListAsync();
            return View(rooms);
        }

        // GET: Rooms/Filter
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

        // GET: Rooms/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new RoomViewModel());
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var room = new Room
            {
                Number = model.Number,
                Type = model.Type,
                Price = model.Price,
                Status = model.Status,
                Capacity = model.Capacity,
                Description = model.Description,
                Amenities = model.Amenities,
                Images = model.ImageUrls?
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .Select(url => new RoomImage { ImageUrl = url })
                    .ToList() ?? new List<RoomImage>()
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.RoomId == id);
            if (room == null) return NotFound();

            var model = new RoomViewModel
            {
                RoomId = room.RoomId,
                Number = room.Number,
                Type = room.Type,
                Price = room.Price,
                Status = room.Status,
                Capacity = room.Capacity,
                Description = room.Description,
                Amenities = room.Amenities,
                ImageUrls = room.Images.Select(img => img.ImageUrl).ToList()
            };

            return View(model);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomViewModel model)
        {
            if (!ModelState.IsValid) { 
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            Console.WriteLine(string.Join("\n", errors));
            return View(model); }


            var room = await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.RoomId == model.RoomId);
            if (room == null) return NotFound();

            room.Number = model.Number;
            room.Type = model.Type;
            room.Price = model.Price;
            room.Status = model.Status;
            room.Capacity = model.Capacity;
            room.Amenities = model.Amenities;
            room.Description = model.Description;

            room.Images.Clear();
            room.Images = model.ImageUrls?
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Select(url => new RoomImage { ImageUrl = url, RoomId = room.RoomId })
                .ToList() ?? new List<RoomImage>();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.RoomId == id);
            if (room == null) return NotFound();

            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.RoomId == id);
            if (room == null) return NotFound();

            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.RoomId == id);
            if (room != null)
            {
                // Видалити файли з папки
                foreach (var image in room.Images)
                {
                    var filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

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