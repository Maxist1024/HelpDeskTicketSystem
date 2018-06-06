using System.ComponentModel.DataAnnotations;
using TicketSystem.Model;

namespace TicketSystem.Models
{
    public class TicketViewModel
    {
        [MinLength(20)]
        [MaxLength(150)]
        public string Title { get; set; }

        [MinLength(50)]
        public string Description { get; set; }

        public TypesOfTicket TypeOfTicket { get; set; }


    }
}