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
  
        public IActionResult Index()
        {
            var reservations = _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .ToList();

            return View(reservations);
        }
    public async Task<IActionResult> AvailableRooms()
    {
        var rooms = await _context.Rooms
            .Include(r => r.Images)
            .ToListAsync();

        return View(rooms);
    }


    [HttpPost]
    public async Task<IActionResult> Book(ReservationViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);

        // Перевірка на конфлікт бронювання
        var conflict = await _context.Reservations.AnyAsync(r =>
            r.RoomId == model.RoomId &&
            r.CheckOut > model.CheckIn &&
            r.CheckIn < model.CheckOut);

        if (conflict)
        {
            ModelState.AddModelError("", "Цей номер вже заброньований на вибрані дати.");
            return View(model);
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

        return RedirectToAction("MyReservations");
    }
    [Authorize]
    public async Task<IActionResult> Create(int roomId)
    {
        var room = await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.RoomId == roomId);
        if (room == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);

        var model = new ReservationViewModel
        {
            RoomId = room.RoomId,
            Room = room,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(1),
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.PhoneNumber
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> MyReservations()
    {
        var user = await _userManager.GetUserAsync(User);
        var reservations = await _context.Reservations
            .Include(r => r.Room)
            .Where(r => r.UserId == user.Id)
            .ToListAsync();

        return View(reservations);
    }
}