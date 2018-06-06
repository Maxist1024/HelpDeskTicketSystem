using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TicketSystem.Model
{
    public class User : IdentityUser
    {
        //public int TicketId { get; set; }

        //public virtual Ticket Ticket { get; set; }

        public TypesOfTicket TypeOfTicket { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }

    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            //HasOptional(t => t.Ticket)
            //    .WithRequired(u => u.User);
        }
    }
}