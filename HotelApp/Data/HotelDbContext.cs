using HotelDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, Number = "101", Type = "Single", Price = 50, Status = "Free" },
                new Room { RoomId = 2, Number = "102", Type = "Double", Price = 80, Status = "Free" },
                new Room { RoomId = 3, Number = "201", Type = "Suite", Price = 150, Status = "Free" }
            );
        }
    }
}
