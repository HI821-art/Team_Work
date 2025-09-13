using HotelApp.Persistance;
using HotelDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelDb.Data
{
    public class HotelDbContext : IdentityDbContext<User>
    {
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public HotelDbContext() { }

        public DbSet<RoomImage> RoomImages { get; set; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
           //this.Database.EnsureDeleted();
           //this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                var str = "workstation id=HotelDb.mssql.somee.com;packet size=4096;user id=marexx00_SQLLogin_1;pwd=njvnxaefpn;data source=HotelDb.mssql.somee.com;persist security info=False;initial catalog=HotelDb;TrustServerCertificate=True";
                optionsBuilder.UseSqlServer(str);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedInitialData();

        }
    }
}
