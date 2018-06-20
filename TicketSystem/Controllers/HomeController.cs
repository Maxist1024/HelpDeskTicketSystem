using System;
using System.Net.Mail;
using System.Web.Mvc;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {

            return View(new ContactMailViewModel());
        }

        [HttpPost]
        public ActionResult Contact(ContactMailViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(vm.Email);//Email which you are getting 
                    //from contact us page 
                    mailMessage.To.Add("ticketsystem.isk@gmail.com");//Where mail will be sent 
                    mailMessage.Subject = "[TicketSystem Contact page] Wiadomość od " + vm.Email;
                    mailMessage.Body = vm.Message;
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        Credentials = new System.Net.NetworkCredential("ticketsystem.isk@gmail.com", "Asdewq123$"),
                        EnableSsl = true
                    };

                    smtp.Send(mailMessage);

                    ModelState.Clear();
                    vm.IsSuccess = true;
                    vm.Response = "Dziękujemy za wiadomość. Administrator wkrótce skontaktuje się z Tobą!";
                } catch (Exception)
                {
                    ModelState.Clear();
                    vm.IsSuccess = false;

                    vm.Response = "Wystąpił problem. Spróbuj ponownie później...";
                }
            }

            return View(vm);
        }


    }
}