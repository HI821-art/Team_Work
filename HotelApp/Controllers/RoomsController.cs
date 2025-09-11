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
            new Room
            {
                RoomId = 1,
                Number = "101",
                Type = "Single",
                Price = 1500.00,
                Status = "Free",
                Description = "Cozy single room with a balcony and city view.",
                Images = new List<RoomImage>
                {
                    new RoomImage { RoomImageId = 1, ImageUrl = "/images/rooms/101_1.jpg" },
                    new RoomImage { RoomImageId = 2, ImageUrl = "/images/rooms/101_2.jpg" }
                }
            },
            new Room
            {
                RoomId = 2,
                Number = "202",
                Type = "Double",
                Price = 2500.00,
                Status = "Free",
                Description = "Spacious double room with modern design and large beds.",
                Images = new List<RoomImage>
                {
                    new RoomImage { RoomImageId = 3, ImageUrl = "/images/rooms/202_1.jpg" },
                    new RoomImage { RoomImageId = 4, ImageUrl = "/images/rooms/202_2.jpg" },
                    new RoomImage { RoomImageId = 20, ImageUrl = "/images/rooms/202_3.jpg" },
                }
            },
            new Room
            {
                RoomId = 3,
                Number = "303",
                Type = "Presidental",
                Price = 5000.00,
                Status = "Booked",
                Description = "Luxury presidential suite with premium facilities and panoramic city view.",
                Images = new List<RoomImage>
                {
                    new RoomImage { RoomImageId = 5, ImageUrl = "/images/rooms/303_1.jpg" },
                    new RoomImage { RoomImageId = 6, ImageUrl = "/images/rooms/303_2.jpg" },
                    new RoomImage { RoomImageId = 17, ImageUrl = "/images/rooms/303_6.jpg" },
                    new RoomImage { RoomImageId = 18, ImageUrl = "/images/rooms/303_4.jpg" },
                    new RoomImage { RoomImageId = 19, ImageUrl = "/images/rooms/303_5.jpg" },
                }
            },
            new Room
            {
                RoomId = 4,
                Number = "104",
                Type = "Single",
                Price = 1800.00,
                Status = "Booked",
                Description = "Comfortable single room with a minimalist interior, perfect for short stays.",
                Images = new List<RoomImage>
                {
                    new RoomImage { RoomImageId = 8, ImageUrl = "/images/rooms/104_1.jpg" },
                    new RoomImage { RoomImageId = 9, ImageUrl = "/images/rooms/104_2.jpg" }
                }
            },
            new Room
            {
                RoomId = 5,
                Number = "205",
                Type = "Double",
                Price = 2800.00,
                Status = "Free",
                Description = "Modern double room with bright colors and a relaxing atmosphere.",
                Images = new List<RoomImage>
                {
                    new RoomImage { RoomImageId = 10, ImageUrl = "/images/rooms/205_1.jpg" },
                    new RoomImage { RoomImageId = 11, ImageUrl = "/images/rooms/205_2.jpg" },
                    new RoomImage { RoomImageId = 21, ImageUrl = "/images/rooms/205_3.jpg" }
                }
            },
            new Room
            {
                RoomId = 6,
                Number = "306",
                Type = "Presidental",
                Price = 5500.00,
                Status = "Free",
                Description = "Exclusive presidential suite with luxury furniture, private bar and Jacuzzi.",
                Images = new List<RoomImage>
                {
                    new RoomImage { RoomImageId = 12, ImageUrl = "/images/rooms/306_1.jpg" },
                    new RoomImage { RoomImageId = 13, ImageUrl = "/images/rooms/306_2.jpg" },
                    new RoomImage { RoomImageId = 14, ImageUrl = "/images/rooms/306_3.jpg" },
                    new RoomImage { RoomImageId = 15, ImageUrl = "/images/rooms/306_4.jpg" },
                    new RoomImage { RoomImageId = 16, ImageUrl = "/images/rooms/306_5.jpg" },
                }
            }
        };

     
        public IActionResult Index()
        {
            return View(_rooms);
        }

        
        public IActionResult Details(int id)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        
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
                existingRoom.Description = room.Description;
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

       
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
