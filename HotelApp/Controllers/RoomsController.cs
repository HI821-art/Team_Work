using System.Linq;
using HotelDb.Models;
using Microsoft.AspNetCore.Mvc;
using HotelDb.Data; // Add this using

namespace HotelApp.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelDbContext _context;

        public RoomsController(HotelDbContext context)
        {
            _context = context;
        }

        // Read: Список номерів
        public IActionResult Index()
        {
            var rooms = _context.Rooms.ToList();
            return View(rooms);
        }

        // Details: Деталі номеру
        public IActionResult Details(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // Create: Форма створення
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"{key}: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Rooms.Add(room);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // Edit: Форма редагування
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingRoom = _context.Rooms.FirstOrDefault(r => r.RoomId == id);
                if (existingRoom == null)
                {
                    return NotFound();
                }
                existingRoom.Number = room.Number;
                existingRoom.Type = room.Type;
                existingRoom.Price = room.Price;
                existingRoom.Status = room.Status;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // Delete: Підтвердження видалення
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}