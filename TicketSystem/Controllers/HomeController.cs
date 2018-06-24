using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
                if (!User.Identity.IsAuthenticated && null == vm.Email)
                {
                    vm.Response = "Mail nie może być pusty.";
                    return View(vm);
                }
                else
                {
                    try
                    {
                        MailMessage mailMessage = new MailMessage();
                        mailMessage.From = new MailAddress("dlaisk@o2.pl");//Email which you are getting 

                        if (User.Identity.IsAuthenticated)
                        {
                            //from contact us page 
                            mailMessage.To.Add("ticketsystem.isk@gmail.com");//Where mail will be sent 
                            mailMessage.Subject = "[TicketSystem Contact page] Wiadomość od " + Task.Run(
                                    async () => await UserManager.GetEmailAsync(
                                        User.Identity.GetUserId())).Result;
                        }
                        else
                        {
                            mailMessage.To.Add("ticketsystem.isk@gmail.com");//Where mail will be sent 
                            mailMessage.Subject = "[TicketSystem Contact page] Wiadomość od " + vm.Email;
                        }

                        mailMessage.Body = vm.Message;
                        SmtpClient smtp = new SmtpClient
                        {
                            Host = "poczta.o2.pl",
                            Port = 587,
                            Credentials = new System.Net.NetworkCredential("dlaisk@o2.pl", "emailisk123"),
                            EnableSsl = true
                        };

                        smtp.Send(mailMessage);

                        ModelState.Clear();
                        vm.IsSuccess = true;
                        vm.Response = "Dziękujemy za wiadomość. Administrator wkrótce skontaktuje się z Tobą!";
                    }
                    catch (Exception ex)
                    {
                        ModelState.Clear();
                        vm.IsSuccess = false;

                        vm.Response = "Wystąpił problem. Spróbuj ponownie później...";
                    }
                }
            }

            return View(vm);
        }


    }
}