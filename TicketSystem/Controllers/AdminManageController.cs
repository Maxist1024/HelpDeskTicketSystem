using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketSystem.DataAccess;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    public class AdminManageController : Controller
    {

        public ActionResult ManageUser()
        {
            List<ManageUserViewModel> users = new List<ManageUserViewModel>();
            using (TicketDbContext context = TicketDbContext.Create())
            {
                var roleAdminId = context.Roles.First(r => r.Name == "Admin").Id;
                users = context.Users.Select(x => new
                {
                    Email = x.Email,
                    TypeOfTicket = x.TypeOfTicket,
                    Roles = (from userRole in x.Roles
                             join role
                             in context.Roles on userRole.RoleId
                             equals role.Id
                             select role.Name).ToList()
                }).ToList().Select(p => new ManageUserViewModel()
                {
                    Email = p.Email,
                    TypeOfTicket = p.TypeOfTicket,
                    Role = string.Join(",", p.Roles)
                }).ToList();
            }
            return View(users.Where(x => x.Role != "Admin").ToList());
        }

        // GET: AdminManage
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminManage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminManage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminManage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminManage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminManage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminManage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
