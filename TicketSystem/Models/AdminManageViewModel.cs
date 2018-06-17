using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketSystem.Model;

namespace TicketSystem.Models
{
    public class ManageUserViewModel
    {      
        public string Id { get; set; }

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

    public class ManageTicketViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Typ zgłoszenia")]
        public TypesOfTicket TypeOfTicket { get; set; }

        [Display(Name = "Osoba zgłaszająca")]
        public string UserName { get; set; }
    }

    public class EditUserViewModel
    {
        [Required]
        [Display(Name = "Użytkownik")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Typ użytkownika")]
        public string Role { get; set; }


        [Display(Name = "Rodzaj zgłoszenia")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public TypesOfTicket TypeOfTicket { get; set; }
    }
}