using System;

namespace HotelDb.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; }   

        // user relation
        public string UserId { get; set; }
        public User User { get; set; }

        // room relation
        public int RoomId { get; set; }
        public Room Room { get; set; }

        // payment relation
        public Payment Payment { get; set; }
    }
}
