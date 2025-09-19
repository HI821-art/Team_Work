using HotelDb.Data;
using HotelDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Вибираємо рядок підключення залежно від середовища
string connectionString;

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("HotelDb")
        ?? throw new InvalidOperationException("Connection string 'HotelDbLocal' not found.");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("HotelDbSomee")
        ?? throw new InvalidOperationException("Connection string 'HotelDbSomee' not found.");
}

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<HotelDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
        db.Database.EnsureCreated();

        // Створюємо ролі та дефолтного адміністратора
        await RoleInitializer.SeedRolesAndAdminAsync(scope.ServiceProvider);
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
