using System.Collections.Generic;

namespace HotelDb.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; } // Single, Double, Presidental
        public double Price { get; set; }
        public string Status { get; set; } // Free / Booked

        public ICollection<Reservation> Reservations { get; set; }
    }
}