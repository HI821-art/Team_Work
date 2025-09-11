using HotelDb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Persistance
{
    public static class Extensions
    {
        public static void SeedInitialData(this ModelBuilder modelBuilder)
        {
            // Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, Number = "101", Type = "Single", Price = 1500.00, Status = "Free", Description = "Cozy single room with a balcony and city view." },
                new Room { RoomId = 2, Number = "202", Type = "Double", Price = 2500.00, Status = "Free", Description = "Spacious double room with modern design and large beds." },
                new Room { RoomId = 3, Number = "303", Type = "Presidental", Price = 5000.00, Status = "Booked", Description = "Luxury presidential suite with premium facilities and panoramic city view." },
                new Room { RoomId = 4, Number = "104", Type = "Single", Price = 1800.00, Status = "Booked", Description = "Comfortable single room with a minimalist interior, perfect for short stays." },
                new Room { RoomId = 5, Number = "205", Type = "Double", Price = 2800.00, Status = "Free", Description = "Modern double room with bright colors and a relaxing atmosphere." },
                new Room { RoomId = 6, Number = "306", Type = "Presidental", Price = 5500.00, Status = "Free", Description = "Exclusive presidential suite with luxury furniture, private bar and Jacuzzi." }
            );

            // Room Images
            modelBuilder.Entity<RoomImage>().HasData(
                // Room 101
                new RoomImage { RoomImageId = 1, RoomId = 1, ImageUrl = "/images/rooms/101_1.jpg" },
                new RoomImage { RoomImageId = 2, RoomId = 1, ImageUrl = "/images/rooms/101_2.jpg" },

                // Room 202
                new RoomImage { RoomImageId = 3, RoomId = 2, ImageUrl = "/images/rooms/202_1.jpg" },
                new RoomImage { RoomImageId = 4, RoomId = 2, ImageUrl = "/images/rooms/202_2.jpg" },
                new RoomImage { RoomImageId = 20, RoomId = 2, ImageUrl = "/images/rooms/202_3.jpg" },

                // Room 303
                new RoomImage { RoomImageId = 5, RoomId = 3, ImageUrl = "/images/rooms/303_1.jpg" },
                new RoomImage { RoomImageId = 6, RoomId = 3, ImageUrl = "/images/rooms/303_2.jpg" },
                new RoomImage { RoomImageId = 17, RoomId = 3, ImageUrl = "/images/rooms/303_6.jpg" },
                new RoomImage { RoomImageId = 18, RoomId = 3, ImageUrl = "/images/rooms/303_4.jpg" },
                new RoomImage { RoomImageId = 19, RoomId = 3, ImageUrl = "/images/rooms/303_5.jpg" },

                // Room 104
                new RoomImage { RoomImageId = 8, RoomId = 4, ImageUrl = "/images/rooms/104_1.jpg" },
                new RoomImage { RoomImageId = 9, RoomId = 4, ImageUrl = "/images/rooms/104_2.jpg" },

                // Room 205
                new RoomImage { RoomImageId = 10, RoomId = 5, ImageUrl = "/images/rooms/205_1.jpg" },
                new RoomImage { RoomImageId = 11, RoomId = 5, ImageUrl = "/images/rooms/205_2.jpg" },
                new RoomImage { RoomImageId = 21, RoomId = 5, ImageUrl = "/images/rooms/205_3.jpg" },

                // Room 306
                new RoomImage { RoomImageId = 12, RoomId = 6, ImageUrl = "/images/rooms/306_1.jpg" },
                new RoomImage { RoomImageId = 13, RoomId = 6, ImageUrl = "/images/rooms/306_2.jpg" },
                new RoomImage { RoomImageId = 14, RoomId = 6, ImageUrl = "/images/rooms/306_3.jpg" },
                new RoomImage { RoomImageId = 15, RoomId = 6, ImageUrl = "/images/rooms/306_4.jpg" },
                new RoomImage { RoomImageId = 16, RoomId = 6, ImageUrl = "/images/rooms/306_5.jpg" }
            );
        }
    }
}
