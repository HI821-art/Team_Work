using System.Collections.Generic;

namespace HotelDb.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; } // Single, Double, Presidential
        public double Price { get; set; }
        public string Status { get; set; } 

        public string? Description { get; set; }

        public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();

        

        public ICollection<Reservation> Reservations { get; set; }
    }

    public class RoomImage
    {
        public int RoomImageId { get; set; }
        public string ImageUrl { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }    }
}

