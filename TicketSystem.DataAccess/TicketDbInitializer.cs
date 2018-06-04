using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Model;

namespace TicketSystem.DataAccess
{
    public class TicketDbInitializer : CreateDatabaseIfNotExists<DbContext>
    {
        protected override void Seed(DbContext context)
        {    
            var UserManager = new UserManager<User>(new UserStore<User>(context));

            var user1 = new User() { UserName = "miszcz", Email = "miszcz@gmail.com", TypeOfTicket = TypesOfTicket.Sprzet};
            string user1PWD = "Miszcz123";
            var chkUser1 = UserManager.Create(user1, user1PWD);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
