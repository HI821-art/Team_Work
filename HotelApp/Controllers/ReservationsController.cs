using HotelDb.Data;
using HotelDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // Main booking page - shows available rooms
    public async Task<IActionResult> Index(DateTime? checkIn, DateTime? checkOut, int guests = 2)
    {
        // Set default dates if not provided
        var defaultCheckIn = checkIn ?? DateTime.Today.AddDays(1);
        var defaultCheckOut = checkOut ?? DateTime.Today.AddDays(2);

        // Get available rooms (not booked for selected dates)
        var bookedRoomIds = await _context.Reservations
            .Where(r => r.CheckOut > defaultCheckIn && r.CheckIn < defaultCheckOut)
            .Select(r => r.RoomId)
            .ToListAsync();

        var availableRooms = await _context.Rooms
            .Include(r => r.Images)
            .Where(r => !bookedRoomIds.Contains(r.RoomId) && r.Capacity >= guests)
            .ToListAsync();

        ViewBag.CheckIn = defaultCheckIn;
        ViewBag.CheckOut = defaultCheckOut;
        ViewBag.Guests = guests;
        ViewBag.TotalNights = (defaultCheckOut - defaultCheckIn).Days;

        return View(availableRooms);
    }

    // Room selection with dates - AJAX endpoint
    [HttpGet]
    public async Task<IActionResult> SearchRooms(DateTime checkIn, DateTime checkOut, int guests = 2)
    {
        var bookedRoomIds = await _context.Reservations
            .Where(r => r.CheckOut > checkIn && r.CheckIn < checkOut)
            .Select(r => r.RoomId)
            .ToListAsync();

        var availableRooms = await _context.Rooms
            .Include(r => r.Images)
            .Where(r => !bookedRoomIds.Contains(r.RoomId) && r.Capacity >= guests)
            .ToListAsync();

        ViewBag.CheckIn = checkIn;
        ViewBag.CheckOut = checkOut;
        ViewBag.Guests = guests;
        ViewBag.TotalNights = (checkOut - checkIn).Days;

        return PartialView("_RoomCards", availableRooms);
    }

    [Authorize]
    public async Task<IActionResult> Create(int roomId, DateTime? checkIn, DateTime? checkOut)
    {
        var room = await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.RoomId == roomId);
        if (room == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);

        var model = new ReservationViewModel
        {
            RoomId = room.RoomId,
            Room = room,
            CheckIn = checkIn ?? DateTime.Today.AddDays(1),
            CheckOut = checkOut ?? DateTime.Today.AddDays(2),
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.PhoneNumber
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Book(ReservationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Room = await _context.Rooms.Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.RoomId == model.RoomId);
            return View("Create", model);
        }

        var user = await _userManager.GetUserAsync(User);

        // Check for booking conflicts
        var conflict = await _context.Reservations.AnyAsync(r =>
            r.RoomId == model.RoomId &&
            r.CheckOut > model.CheckIn &&
            r.CheckIn < model.CheckOut);

        if (conflict)
        {
            ModelState.AddModelError("", "This room is already booked for the selected dates.");
            model.Room = await _context.Rooms.Include(r => r.Images)
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

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Your reservation has been confirmed!";
        return RedirectToAction("MyReservations");
    }

    [HttpGet]
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
        var reservations = _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.User)
            .OrderByDescending(r => r.CheckIn)
            .ToList();

        return View(reservations);
    }
}