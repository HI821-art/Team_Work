using System;

namespace HotelDb.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public string Method { get; set; } 
        public DateTime Date { get; set; }

        // reservation relation
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
