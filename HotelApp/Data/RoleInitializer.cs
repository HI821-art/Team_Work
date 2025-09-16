using HotelDb.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelDb.Data
{
    public static class RoleInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            // Create defoult admin 
            var adminEmail = "admin@hotel.com";

            if (await (userManager.FindByEmailAsync(adminEmail)) == null)
            {
                var user = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Default Admin",
                    Birthdate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {
                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
