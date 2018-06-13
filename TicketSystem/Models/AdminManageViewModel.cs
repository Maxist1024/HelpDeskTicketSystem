using System.ComponentModel.DataAnnotations;
using TicketSystem.Model;

namespace TicketSystem.Models
{
    public class ManageUserViewModel
    {
        [Required]
        [Display(Name = "Adres e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Preferowany typ ticketu")]
        [EnumDataType(typeof(TypesOfTicket))]
        public TypesOfTicket TypeOfTicket { get; set; }

        [Required]
        [Display(Name = "Typ użytkownika")]
        public string Role { get; set; }
    }
}