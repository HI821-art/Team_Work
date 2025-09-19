using System;
using System.ComponentModel.DataAnnotations;
namespace HotelDb.Models
{
    public class ReservationViewModel
    {
        [Key]
        public int Id { get; set; }

        public int RoomId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
        // Автозаповнені дані користувача
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        // Для відображення інформації про номер
        public Room Room { get; set; }
        // Розрахунок суми
        public double TotalPrice => Room != null && CheckOut > CheckIn
            ? (CheckOut - CheckIn).Days * Room.Price
            : 0;
    }
}