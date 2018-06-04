using System;
using System.Data.Entity.ModelConfiguration;

namespace TicketSystem.Model
{
    public class Ticket
    {
        public int TicketId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TypesOfTicket TypeOfTicket{ get; set; }

        public StatusesOfTicket StatusOfTicket { get; set; }

        public DateTime CreationTime { get; set; }

        public virtual User User { get; set; }
    } 

    public class TicketConfig : EntityTypeConfiguration<Ticket>
    {
        public TicketConfig()
        {
            HasKey(t => t.TicketId);
            Property(t => t.Title).IsRequired();
            Property(t => t.Description).IsRequired();
            Property(t => t.CreationTime).IsRequired();
            Property(t => t.TypeOfTicket).IsRequired();
            Property(t => t.StatusOfTicket).IsRequired();
            ToTable("Ticket");
        }
    }
}
