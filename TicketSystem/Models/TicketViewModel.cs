using System.ComponentModel.DataAnnotations;
using TicketSystem.Model;

namespace TicketSystem.Models
{
    public class TicketViewModel
    {
        [Display(Name = "Tytuł")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Tytuł powinień zawierać {2}-{1} znaków.")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string Title { get; set; }


        [Display(Name = "Opis")]
        [StringLength(200, MinimumLength = 25, ErrorMessage = "Opis powinień zawierać {2}-{1} znaków.")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string Description { get; set; }

        [Display(Name = "Rodzaj zgłoszenia")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public TypesOfTicket TypeOfTicket { get; set; }

        public bool IsSuccess { get; set; }
        //public string Message { get; set; }

        public TicketViewModel(bool isSuccess)
        {
            IsSuccess = true;//if send successfully to the database
        }

        public TicketViewModel()
        {

        }
    }
}