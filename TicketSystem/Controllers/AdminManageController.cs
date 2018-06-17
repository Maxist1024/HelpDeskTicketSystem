using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketSystem.DataAccess;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminManageController : Controller
    {
        #region Ctr
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
                    Id= p.Id,
                    Email = p.Email,
                    TypeOfTicket = p.TypeOfTicket,
                    Role = string.Join(",", p.Roles)
                }).ToList();
            }
            return View(users.Where(x => x.Role != "Admin").ToList());
        }

        // GET: AdminManage/EditUser/
        public ActionResult EditUser(string id)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Helper", Value = "0" });

            items.Add(new SelectListItem { Text = "User", Value = "1", Selected = true });

            ViewBag.Roles = items;

            var user = UserManager.FindById(id);

            EditUserViewModel editUser = new EditUserViewModel() { Email = user.Email };

            return View(editUser);
        }

        // POST: AdminManage/EditUser/
        [HttpPost]
        public ActionResult EditUser(string id, EditUserViewModel collection, string Roles)
        {
            try
            {
                using (TicketDbContext context = TicketDbContext.Create())
                {
                    var user = UserManager.FindById(id);
                    IdentityRole identityRole = new IdentityRole();
                    if (Roles == "0")
                    {
                        identityRole = context.Roles.First(r => r.Name == "Helper");
                        UserManager.RemoveFromRole(user.Id, "User");
                        UserManager.AddToRole(user.Id, "Helper");
                    }
                    else
                    {
                        identityRole = context.Roles.First(r => r.Name == "User");
                        UserManager.RemoveFromRole(user.Id, "Helper");
                        UserManager.AddToRole(user.Id, "User");
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


        // GET: AdminManage/Delete/5
        public ActionResult DeleteUser(string id)
        {
            var user = UserManager.FindById(id);
            if(null == user)
            {
                return new HttpNotFoundResult("Specjalnie tu trafiłeś?");
            }
            var model = new ManageUserViewModel() { Email = user.Email, TypeOfTicket = user.TypeOfTicket, Id = user.Id };
            return View(model);
        }

        // POST: AdminManage/Delete/5
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
                       UserName = x.User.UserName
                   }).ToList();
            }
            return View(tickets);
        }
    }
}
