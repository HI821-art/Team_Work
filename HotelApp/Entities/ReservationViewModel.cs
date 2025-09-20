using System;
using System.ComponentModel.DataAnnotations;

namespace HotelDb.Models
{
    public class ReservationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Необхідно обрати номер")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Дата заїзду обов'язкова")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата заїзду")]
        public DateTime CheckIn { get; set; }

        [Required(ErrorMessage = "Дата виїзду обов'язкова")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата виїзду")]
        public DateTime CheckOut { get; set; }

        // Автозаповнені дані користувача
        [Required(ErrorMessage = "Повне ім'я обов'язкове")]
        [StringLength(100, ErrorMessage = "Ім'я не може бути довшим за 100 символів")]
        [Display(Name = "Повне ім'я")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Неправильний формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Неправильний формат телефону")]
        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        // Для відображення інформації про номер
        public Room? Room { get; set; }

        // Розрахунок суми
        public double TotalPrice => Room != null && CheckOut > CheckIn
            ? (CheckOut - CheckIn).Days * Room.Price
            : 0;

        // Валідація дат
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckIn < DateTime.Today)
            {
                yield return new ValidationResult(
                    "Дата заїзду не може бути в минулому",
                    new[] { nameof(CheckIn) });
            }

            if (CheckOut <= CheckIn)
            {
                yield return new ValidationResult(
                    "Дата виїзду повинна бути пізніше дати заїзду",
                    new[] { nameof(CheckOut) });
            }

            if ((CheckOut - CheckIn).Days > 30)
            {
                yield return new ValidationResult(
                    "Максимальний період бронювання - 30 днів",
                    new[] { nameof(CheckOut) });
            }
        }
    }
}