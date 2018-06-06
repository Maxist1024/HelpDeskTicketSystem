using System.Web.Mvc;
using TicketSystem.DataAccess;

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
    }
}