using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{

    //zakładka Kontakt
    public class ContactMailViewModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [EmailAddress]
        [StringLength(200, MinimumLength =1 )]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Treść")]
        public string Message { get; set; }

        public string Response { get; set; }

        public bool IsSuccess { get; set; }

        public ContactMailViewModel()
        {
            IsSuccess = false;
            Response = "";
        }
    }


}
