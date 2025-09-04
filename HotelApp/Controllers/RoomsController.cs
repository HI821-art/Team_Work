using System.Collections.Generic;
using System.Linq;
using HotelDb.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    public class RoomsController : Controller
    {
        private static List<Room> _rooms = new List<Room>
        {
            new Room { RoomId = 1, Number = "101", Type = "Single", Price = 1500.00, Status = "Free" },
            new Room { RoomId = 2, Number = "202", Type = "Double", Price = 2500.00, Status = "Free" },
            new Room { RoomId = 3, Number = "303", Type = "Presidental", Price = 5000.00, Status = "Booked" },
            new Room { RoomId = 4, Number = "104", Type = "Single", Price = 1800.00, Status = "Booked" },
            new Room { RoomId = 5, Number = "205", Type = "Double", Price = 2800.00, Status = "Free" },
            new Room { RoomId = 6, Number = "306", Type = "Presidental", Price = 5500.00, Status = "Free" }
        };

        // Read: Список номерів
        public IActionResult Index()
        {
            return View(_rooms);
        }

        // Details: Деталі номеру
        public IActionResult Details(int id)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == id);
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
            if (ModelState.IsValid)
            {
                room.RoomId = _rooms.Any() ? _rooms.Max(r => r.RoomId) + 1 : 1;
                _rooms.Add(room);
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // Edit: Форма редагування
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == id);
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
                var existingRoom = _rooms.FirstOrDefault(r => r.RoomId == id);
                if (existingRoom == null)
                {
                    return NotFound();
                }
                existingRoom.Number = room.Number;
                existingRoom.Type = room.Type;
                existingRoom.Price = room.Price;
                existingRoom.Status = room.Status;
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // Delete: Підтвердження видалення
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == id);
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
            var room = _rooms.FirstOrDefault(r => r.RoomId == id);
            if (room != null)
            {
                _rooms.Remove(room);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}