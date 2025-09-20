using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelDb.Models
{
    public class RoomViewModel
    {
        public int RoomId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]?\d{1,4}$", ErrorMessage = "Invalid room number format.")]
        public string Number { get; set; }

        [Required]
        public string Type { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        public string Status { get; set; }

        [Range(1, 6)]
        public int Capacity { get; set; }

        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Amenities list is too long.")]
        public string? Amenities { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();

        // Додаємо поле для останньої броні
        public DateTime? LastBookedUntil { get; set; }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

}