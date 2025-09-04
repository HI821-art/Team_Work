using HotelApp.Persistance;
using HotelDb.Models;
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

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
            // this.Database.EnsureDeleted();
            // this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                var str = "Server=(localdb)\\mssqllocaldb;Database=HotelDb;Trusted_Connection=True;MultipleActiveResultSets=true";
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
