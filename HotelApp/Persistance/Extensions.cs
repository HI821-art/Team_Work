using HotelDb.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace HotelApp.Persistance
{
    public static class Extensions
    {
        public static void SeedInitialData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, Number = "101",  Type = "Single", Price = 1500.00, Status = "Free" },
                new Room { RoomId = 2, Number = "202", Type = "Double", Price = 2500.00, Status = "Free" },
                new Room { RoomId = 3, Number = "303",  Type = "Presidental", Price = 5000.00, Status = "Booked" },
                new Room { RoomId = 4, Number = "104", Type = "Single", Price = 1800.00, Status = "Booked" },
                new Room { RoomId = 5, Number = "205", Type = "Double", Price = 2800.00, Status = "Free" },
                new Room { RoomId = 6, Number = "306", Type = "Presidental", Price = 5500.00, Status = "Free" }
                );
    
        }

    }
}