using HotelDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelDb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // List all users
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var currentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserId = currentUserId; // Pass current admin ID to the view
            return View(users);
        }

        // Create user (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Create user (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return View();

            var user = new User { UserName = email, Email = email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(role))
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                        await _roleManager.CreateAsync(new IdentityRole(role));

                    await _userManager.AddToRoleAsync(user, role);
                }
                TempData["Success"] = $"User {email} created successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View();
        }

        // Delete user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUserId = _userManager.GetUserId(User);

            // Prevent self-delete
            if (id == currentUserId)
            {
                TempData["Error"] = "You cannot delete your own account while logged in.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                TempData["Success"] = $"User {user.Email} deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
