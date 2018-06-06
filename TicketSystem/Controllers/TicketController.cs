using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using TicketSystem.DataAccess;
using TicketSystem.Model;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    public class TicketController : Controller
    {

        private TicketDbContext _context;

        public TicketController()
        {
            _context = TicketDbContext.Create();
        }


        // GET: Zwaraca formularz do wypełnienia zgłoszenia
        // [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        // [Authorize]
        public ActionResult Add(TicketViewModel ticketForm)
        {
            if (!ModelState.IsValid)
                return View(ticketForm);

            var userId = User.Identity.GetUserId();
            var ticketToDb = new Ticket
            {
                //TicketId = 3,
                UserId = userId,

                Title = ticketForm.Title,
                Description = ticketForm.Description,
                TypeOfTicket = ticketForm.TypeOfTicket,
                StatusOfTicket = StatusesOfTicket.Zalozony,
                CreationTime = DateTime.Now,

            };

            _context.Tickets.Add(ticketToDb);
            _context.SaveChanges();
            return View();
        }
    }
}