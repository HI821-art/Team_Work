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
                new Room { RoomId = 1, Number = 101, Photo = "https://cdn-ijnhp.nitrocdn.com/pywIAllcUPgoWDXtkiXtBgvTOSromKIg/assets/images/optimized/rev-5794eaa/www.jaypeehotels.com/blog/wp-content/uploads/2024/09/Blog-6-scaled.jpg", Type = "Single", Price = 1500.00, Status = "Free" },
                new Room { RoomId = 2, Number = 202, Photo = "https://cdn-ijnhp.nitrocdn.com/pywIAllcUPgoWDXtkiXtBgvTOSromKIg/assets/images/optimized/rev-5794eaa/www.jaypeehotels.com/blog/wp-content/uploads/2024/09/Blog-6-scaled.jpg", Type = "Double", Price = 2500.00, Status = "Free" },
                new Room { RoomId = 3, Number = 303, Photo = "https://cdn-ijnhp.nitrocdn.com/pywIAllcUPgoWDXtkiXtBgvTOSromKIg/assets/images/optimized/rev-5794eaa/www.jaypeehotels.com/blog/wp-content/uploads/2024/09/Blog-6-scaled.jpg", Type = "Presidental", Price = 5000.00, Status = "Booked" },
                new Room { RoomId = 4, Number = 104, Photo = "https://cdn-ijnhp.nitrocdn.com/pywIAllcUPgoWDXtkiXtBgvTOSromKIg/assets/images/optimized/rev-5794eaa/www.jaypeehotels.com/blog/wp-content/uploads/2024/09/Blog-6-scaled.jpg", Type = "Single", Price = 1800.00, Status = "Booked" },
                new Room { RoomId = 5, Number = 205, Photo = "https://cdn-ijnhp.nitrocdn.com/pywIAllcUPgoWDXtkiXtBgvTOSromKIg/assets/images/optimized/rev-5794eaa/www.jaypeehotels.com/blog/wp-content/uploads/2024/09/Blog-6-scaled.jpg", Type = "Double", Price = 2800.00, Status = "Free" },
                new Room { RoomId = 6, Number = 306, Photo = "https://cdn-ijnhp.nitrocdn.com/pywIAllcUPgoWDXtkiXtBgvTOSromKIg/assets/images/optimized/rev-5794eaa/www.jaypeehotels.com/blog/wp-content/uploads/2024/09/Blog-6-scaled.jpg", Type = "Presidental", Price = 5500.00, Status = "Free" }
                );
    
        }

    }
}