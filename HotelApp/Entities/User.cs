using Microsoft.AspNetCore.Identity;

namespace HotelDb.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
 
        public ICollection<Reservation> Reservations { get; set; }
    }
}
