using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TicketSystem.Model;

namespace TicketSystem.DataAccess
{
    public class TicketDbContext : IdentityDbContext<User>
    {
        public TicketDbContext() : base("TicketSystemDatabase")
        {

        }

        public DbSet<Ticket> Tickets { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TicketConfig());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public static TicketDbContext Create()
        {
            return new TicketDbContext();
        }
    }
}
