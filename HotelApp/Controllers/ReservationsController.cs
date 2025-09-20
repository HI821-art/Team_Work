using HotelDb.Data;
using HotelDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReservationsController(HotelDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations - Main reservation page with available rooms
        public async Task<IActionResult> Index(DateTime? checkIn, DateTime? checkOut, int guests = 1)
        {
            var defaultCheckIn = checkIn ?? DateTime.Today.AddDays(1);
            var defaultCheckOut = checkOut ?? DateTime.Today.AddDays(2);

            if (defaultCheckIn < DateTime.Today)
                defaultCheckIn = DateTime.Today.AddDays(1);

            if (defaultCheckOut <= defaultCheckIn)
                defaultCheckOut = defaultCheckIn.AddDays(1);

            var rooms = await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Reservations)
                .OrderBy(r => r.Type)
                .ThenBy(r => r.Price)
                .ToListAsync();

            // Replace this block inside Index action:
            var roomViewModels = rooms.Select(r =>
            {
                var lastBooking = r.Reservations
                    .Where(res => res.Status != "Cancelled" && res.CheckOut >= DateTime.Today)
                    .OrderByDescending(res => res.CheckOut)
                    .FirstOrDefault();

                return new RoomViewModel
                {
                    RoomId = r.RoomId,
                    Number = r.Number,
                    Type = r.Type,
                    Price = r.Price,
                    Capacity = r.Capacity,
                    Status = r.Status,
                    Description = r.Description,
                    Amenities = r.Amenities,
                    ImageUrls = r.Images.Select(i => i.ImageUrl).ToList(),
                    Reservations = _context.Reservations
                    .Where(res => res.RoomId == r.RoomId && res.Status != "Cancelled")
                    .ToList(),
                    LastBookedUntil = _context.Reservations
                    .Where(res => res.RoomId == r.RoomId && res.Status != "Cancelled")
                    .OrderByDescending(res => res.CheckOut)
                    .Select(res => (DateTime?)res.CheckOut)
                    .FirstOrDefault()
                };
            }).ToList();

            ViewBag.CheckIn = defaultCheckIn;
            ViewBag.CheckOut = defaultCheckOut;
            ViewBag.Guests = guests;
            ViewBag.TotalNights = (defaultCheckOut - defaultCheckIn).Days;

            return View(roomViewModels);
        }


        // AJAX endpoint for searching rooms
        [HttpGet]
        public async Task<IActionResult> SearchRooms(DateTime checkIn, DateTime checkOut, int guests = 1)
        {
            if (checkIn < DateTime.Today)
                checkIn = DateTime.Today.AddDays(1);

            if (checkOut <= checkIn)
                checkOut = checkIn.AddDays(1);

            var bookedRoomIds = await _context.Reservations
                .Where(r => r.CheckOut > checkIn && r.CheckIn < checkOut && r.Status != "Cancelled")
                .Select(r => r.RoomId)
                .ToListAsync();

            var availableRooms = await _context.Rooms
                .Include(r => r.Images)
                .Where(r => !bookedRoomIds.Contains(r.RoomId)
                           && r.Capacity >= guests
                           && r.Status == "Free")
                .OrderBy(r => r.Type)
                .ThenBy(r => r.Price)
                .ToListAsync();

            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.Guests = guests;
            ViewBag.TotalNights = (checkOut - checkIn).Days;

            return PartialView("_RoomCards", availableRooms);
        }

        // GET: Reservations/Create - Reservation form
        public async Task<IActionResult> Create(int roomId, DateTime? checkIn, DateTime? checkOut, int guests = 1)
        {
            var room = await _context.Rooms
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (room == null)
                return NotFound("Room not found");

            var user = await _userManager.GetUserAsync(User);

            var validCheckIn = checkIn ?? DateTime.Today.AddDays(1);
            var validCheckOut = checkOut ?? DateTime.Today.AddDays(2);

            if (validCheckIn < DateTime.Today)
                validCheckIn = DateTime.Today.AddDays(1);

            if (validCheckOut <= validCheckIn)
                validCheckOut = validCheckIn.AddDays(1);


            var model = new ReservationViewModel
            {
                RoomId = room.RoomId,
                Room = room,
                CheckIn = validCheckIn,
                CheckOut = validCheckOut,
                FullName = user.FullName ?? "",
                Email = user.Email ?? "",
                Phone = user.PhoneNumber ?? ""
            };

            return View(model);
        }

        // POST: Reservations/Book - Confirm reservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Room = await _context.Rooms
                    .Include(r => r.Images)
                    .FirstOrDefaultAsync(r => r.RoomId == model.RoomId);
                return View("Create", model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (model.CheckIn < DateTime.Today)
            {
                ModelState.AddModelError("CheckIn", "Check-in date cannot be in the past");
                model.Room = await _context.Rooms
                    .Include(r => r.Images)
                    .FirstOrDefaultAsync(r => r.RoomId == model.RoomId);
                return View("Create", model);
            }

            if (model.CheckOut <= model.CheckIn)
            {
                ModelState.AddModelError("CheckOut", "Check-out date must be after check-in date");
                model.Room = await _context.Rooms
                    .Include(r => r.Images)
                    .FirstOrDefaultAsync(r => r.RoomId == model.RoomId);
                return View("Create", model);
            }

            var conflict = await _context.Reservations.AnyAsync(r =>
                r.RoomId == model.RoomId &&
                r.CheckOut > model.CheckIn &&
                r.CheckIn < model.CheckOut &&
                r.Status != "Cancelled");

            if (conflict)
            {
                ModelState.AddModelError("", "This room is already booked for the selected dates.");
                model.Room = await _context.Rooms
                    .Include(r => r.Images)
                    .FirstOrDefaultAsync(r => r.RoomId == model.RoomId);
                return View("Create", model);
            }

            var reservation = new Reservation
            {
                RoomId = model.RoomId,
                UserId = user.Id,
                CheckIn = model.CheckIn,
                CheckOut = model.CheckOut,
                Status = "Confirmed"
            };

            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Your reservation has been confirmed!";
                return RedirectToAction("MyReservations");
            }
            catch
            {
                ModelState.AddModelError("", "Error creating reservation. Please try again.");
                model.Room = await _context.Rooms
                    .Include(r => r.Images)
                    .FirstOrDefaultAsync(r => r.RoomId == model.RoomId);
                return View("Create", model);
            }
        }

        // GET: Reservations/MyReservations - My reservations
        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.GetUserAsync(User);
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .ThenInclude(r => r.Images)
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.CheckIn)
                .ToListAsync();

            return View(reservations);
        }

        // Admin view for all reservations
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> All()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .OrderByDescending(r => r.CheckIn)
                .ToListAsync();

            return View(reservations);
        }

        // POST: Reservations/Cancel - Cancel reservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.ReservationId == id && r.UserId == user.Id);

            if (reservation == null)
                return NotFound();

            if (reservation.CheckIn <= DateTime.Today)
            {
                TempData["Error"] = "Cannot cancel a reservation after the check-in date";
                return RedirectToAction("MyReservations");
            }

            reservation.Status = "Cancelled";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Reservation cancelled";
            return RedirectToAction("MyReservations");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Extend(int reservationId, DateTime newCheckOut)
        {
            var user = await _userManager.GetUserAsync(User);
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId && r.UserId == user.Id);

            if (reservation == null)
                return NotFound();

            if (newCheckOut <= reservation.CheckOut)
            {
                TempData["Error"] = "New check-out date must be after current check-out.";
                return RedirectToAction("MyReservations");
            }

            var conflict = await _context.Reservations.AnyAsync(r =>
                r.RoomId == reservation.RoomId &&
                r.CheckIn < newCheckOut &&
                r.CheckOut > reservation.CheckOut &&
                r.Status != "Cancelled" &&
                r.ReservationId != reservationId);

            if (conflict)
            {
                TempData["Error"] = "Cannot extend reservation. Room is already booked for these dates.";
                return RedirectToAction("MyReservations");
            }

            reservation.CheckOut = newCheckOut;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Reservation extended successfully.";
            return RedirectToAction("MyReservations");
        }
        [HttpGet]
        public async Task<IActionResult> BookedDates(int roomId)
        {
            var reservations = await _context.Reservations
                .Where(r => r.RoomId == roomId && r.Status != "Cancelled")
                .Select(r => new { r.CheckIn, r.CheckOut })
                .ToListAsync();

            return Json(reservations);
        }

    }
}
