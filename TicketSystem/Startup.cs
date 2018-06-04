using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;
using TicketSystem.DataAccess;
using TicketSystem.Model;

[assembly: OwinStartupAttribute(typeof(TicketSystem.Startup))]
namespace TicketSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }


        private void createRolesandUsers()
        {
            TicketDbContext context = new TicketDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var UserManager = new ApplicationUserManager(new UserStore<User>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new User { UserName = "admin", Email = "admin@gmail.com" };

                string userPWD = "Admin123";

                var chkUser = UserManager.Create(user, userPWD);

                if (chkUser.Succeeded)
                    UserManager.AddToRole(user.Id, "Admin");
            }

            if (!roleManager.RoleExists("Helper"))
            {
                var role = new IdentityRole();
                role.Name = "Helper";
                roleManager.Create(role);

                var collection = context.Users.Where(u => u.UserName != "admin").ToList();
                foreach (var item in collection)
                {
                    UserManager.AddToRole(item.Id, "Helper");
                }
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}