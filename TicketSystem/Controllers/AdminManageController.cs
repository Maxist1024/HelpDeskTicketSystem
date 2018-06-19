using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketSystem.DataAccess;
using TicketSystem.Model;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{

    public class AdminManageController : Controller
    {
        #region Ctor
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminManageController()
        {
        }

        public AdminManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        #region UserArea
        [Authorize(Roles = "Admin")]
        public ActionResult ManageUser()
        {
            List<ManageUserViewModel> users = new List<ManageUserViewModel>();
            using (TicketDbContext context = TicketDbContext.Create())
            {
                var roleAdminId = context.Roles.First(r => r.Name == "Admin").Id;
                users = context.Users.Select(x => new
                {
                    Id = x.Id,
                    Email = x.Email,
                    TypeOfTicket = x.TypeOfTicket,
                    Roles = (from userRole in x.Roles
                             join role
                             in context.Roles on userRole.RoleId
                             equals role.Id
                             select role.Name).ToList()
                }).ToList().Select(p => new ManageUserViewModel()
                {
                    Id = p.Id,
                    Email = p.Email,
                    TypeOfTicket = p.TypeOfTicket,
                    Role = string.Join(",", p.Roles)
                }).ToList();
            }
            users = users.Where(x => x.Role != "Admin").ToList();
            foreach (var user in users)
            {
                if (user.Role == "User")
                    user.TypeOfTicket = null;
            }
            return View(users);
        }

        // GET: AdminManage/EditUser/
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Helper", Value = "0" });

            items.Add(new SelectListItem { Text = "User", Value = "1", Selected = true });

            items.Add(new SelectListItem { Text = "Admin", Value = "2" });

            ViewBag.Roles = items;

            var user = UserManager.FindById(id);

            EditUserViewModel editUser = new EditUserViewModel();

            using (var context = TicketDbContext.Create())
            {
                editUser = new EditUserViewModel()
                {
                    Email = user.Email,
                };
            }
            return View(editUser);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditUserAdvande(string id)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Helper", Value = "0", Selected = true });

            items.Add(new SelectListItem { Text = "User", Value = "1" });

            items.Add(new SelectListItem { Text = "Admin", Value = "2" });

            ViewBag.Roles = items;

            var user = UserManager.FindById(id);

            EditUserViewModel editUser = new EditUserViewModel();

            using (var context = TicketDbContext.Create())
            {
                editUser = new EditUserViewModel()
                {
                    Email = user.Email,
                    TypeOfTicket = user.TypeOfTicket
                };
            }
            return View(editUser);
        }

        // POST: AdminManage/EditUser/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditUser(string id, EditUserViewModel collection, string Roles)
        {
            try
            {
                using (TicketDbContext context = TicketDbContext.Create())
                {
                    var user = UserManager.FindById(id);

                    if (Roles == "0")
                    {
                        var collec = UserManager.GetRoles(user.Id);
                        RemoveRolesFromUser(user, collec);
                        UserManager.AddToRole(user.Id, "Helper");
                    }
                    else if (Roles == "1")
                    {
                        var collec = UserManager.GetRoles(user.Id);
                        RemoveRolesFromUser(user, collec);
                        UserManager.AddToRole(user.Id, "User");
                    }
                    else
                    {
                        var collec = UserManager.GetRoles(user.Id);
                        RemoveRolesFromUser(user, collec);
                        UserManager.AddToRole(user.Id, "Admin");
                    }
                    context.SaveChanges();
                }
                return RedirectToAction("ManageUser");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditUserAdvande(string id, EditUserViewModel collection, string Roles)
        {
            try
            {
                using (TicketDbContext context = TicketDbContext.Create())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);

                    if (Roles == "0")
                    {
                        var collec = UserManager.GetRoles(user.Id);
                        RemoveRolesFromUser(user, collec);
                        UserManager.AddToRole(user.Id, "Helper");
                    }
                    else if (Roles == "1")
                    {
                        var collec = UserManager.GetRoles(user.Id);
                        RemoveRolesFromUser(user, collec);
                        UserManager.AddToRole(user.Id, "User");
                    }
                    else
                    {
                        var collec = UserManager.GetRoles(user.Id);
                        RemoveRolesFromUser(user, collec);
                        UserManager.AddToRole(user.Id, "Admin");
                    }
                    user.TypeOfTicket = collection.TypeOfTicket;

                    context.SaveChanges();
                }
                return RedirectToAction("ManageUser");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminManage/Delete/
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(string id)
        {
            var user = UserManager.FindById(id);
            if (null == user)
            {
                return new HttpNotFoundResult("Specjalnie tu trafiłeś?");
            }
            var model = new ManageUserViewModel() { Email = user.Email, TypeOfTicket = user.TypeOfTicket, Id = user.Id };
            return View(model);
        }

        // POST: AdminManage/Delete/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteUser(string id, FormCollection collection)
        {
            try
            {
                UserManager.Delete(UserManager.FindById(id));

                return RedirectToAction("ManageUser");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region TicketArea
        [Authorize(Roles = "Helper, Admin")]
        public ActionResult ManageTicket()
        {
            List<ManageTicketViewModel> tickets = new List<ManageTicketViewModel>();
            using (TicketDbContext context = TicketDbContext.Create())
            {
                tickets = context.Tickets.Select(x
                   => new ManageTicketViewModel()
                   {
                       Title = x.Title,
                       TypeOfTicket = x.TypeOfTicket,
                       UserName = x.User.UserName,
                       Id = x.TicketId
                   }).ToList();
            }
            return View(tickets);
        }

        [Authorize(Roles = "Helper, Admin")]
        public ActionResult DetailsTicket(int id)
        {
            List<Ticket> ticket = new List<Ticket>();
            using (var context = TicketDbContext.Create())
            {
                ticket = context.Tickets.Where(x => x.TicketId == id).ToList();
            }
            if (null == ticket)
                return new HttpNotFoundResult("Specjalnie tu trafiłeś?");

            return View(ticket);
        }

        [Authorize(Roles = "Helper, Admin")]
        public ActionResult EditTicket(int id)
        {
            EditTicketViewModel ticketToEdit = new EditTicketViewModel();
            using (var context = TicketDbContext.Create())
            {
                ticketToEdit = context.Tickets.Where(x => x.TicketId == id)
                    .Select(x => new EditTicketViewModel()
                    {
                        Id = x.TicketId,
                        StatusOfTicket = x.StatusOfTicket,
                        Title = x.Title,
                        TypeOfTicket = x.TypeOfTicket,
                        UserName = x.User.UserName
                    }).FirstOrDefault();
            }
            if (null == ticketToEdit)
                return new HttpNotFoundResult("Specjalnie tu trafiłeś?");

            return View(ticketToEdit);
        }

        [Authorize(Roles = "Helper, Admin")]
        [HttpPost]
        public ActionResult EditTicket(int id, EditTicketViewModel collection)
        {
            try
            {
                using (var context = TicketDbContext.Create())
                {
                    Ticket ticket = new Ticket();
                    ticket = context.Tickets.FirstOrDefault(x => x.TicketId == id);

                    if (null == ticket)
                        return new HttpNotFoundResult("Specjalnie tu trafiłeś?");

                    ticket.TypeOfTicket = collection.TypeOfTicket;
                    ticket.StatusOfTicket = collection.StatusOfTicket;

                    context.SaveChanges();
                }

                return RedirectToAction("ManageTicket");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Helper, Admin")]
        public ActionResult DeleteTicket(int id)
        {
            DeleteTicketViewModel deleteTicket = new DeleteTicketViewModel();

            using (var context = TicketDbContext.Create())
            {
                deleteTicket = context.Tickets.Where(x => x.TicketId == id)
                    .Select(x => new DeleteTicketViewModel()
                    {
                        Id = x.TicketId,
                        StatusOfTicket = x.StatusOfTicket,
                        Title = x.Title,
                        TypeOfTicket = x.TypeOfTicket,
                        UserName = x.User.UserName,
                        Description = x.Description
                    }).FirstOrDefault();
            }
            if (null == deleteTicket)
                return new HttpNotFoundResult("Specjalnie tu trafiłeś?");

            return View(deleteTicket);
        }

        [Authorize(Roles = "Admin, Helper")]
        [HttpPost]
        public ActionResult DeleteTicket(int id, FormCollection collection)
        {
            try
            {
                Ticket deleteTicket = new Ticket();

                using (var context = TicketDbContext.Create())
                {
                    deleteTicket = context.Tickets.FirstOrDefault(x => x.TicketId == id);

                    if (null == deleteTicket)
                        return new HttpNotFoundResult("Specjalnie tu trafiłeś?");

                    context.Tickets.Remove(deleteTicket);
                    context.SaveChanges();
                }

                return RedirectToAction("ManageTicket");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region tools
        private void RemoveRolesFromUser(User user, IList<string> collec)
        {
            foreach (var item in collec)
            {
                UserManager.RemoveFromRole(user.Id, item);
            }
        }
        #endregion
    }
}
