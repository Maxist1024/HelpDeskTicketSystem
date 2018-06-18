using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{

    //zakładka Kontakt
    public class ContactMailViewModel
    {
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Treść")]
        public string Message { get; set; }

    }
}