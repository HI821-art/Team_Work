namespace HotelDb.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}