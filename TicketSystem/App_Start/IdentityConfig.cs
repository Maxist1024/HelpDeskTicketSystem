using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketSystem.DataAccess;
using TicketSystem.Model;

namespace TicketSystem
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Dołącz tutaj usługę poczty e-mail, aby wysłać wiadomość e-mail.
            CreateSendItem(message);
        }

        public void CreateSendItem(IdentityMessage message)
        {
            SmtpClient smtpClient = new SmtpClient("poczta.o2.pl", 587); //tworzymy klienta smtp
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();  //tworzymy wiadomość
            MailAddress from = new MailAddress("dlaisk@o2.pl", "HelpDeskTicketSystem");//adres nadawcy i nazwa nadawcy

            mail.From = from;
            mail.To.Add(message.Destination);//adres odbiorcy
            mail.Subject = message.Subject;//temat wiadomości
            mail.Body = message.Body; //treść wiadomości

            smtpClient.Credentials = new System.Net.NetworkCredential("dlaisk@o2.pl", "dlaisk123");//nazwa nadawcy i hasło

            try
            {
                smtpClient.Send(mail);//nazwa odbiorcy, wysyłamy wiadomość
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("Klient SMTP wywołał wyjątek. Sprawdź połączenie z internetem." + ex.Message);
            }
        }
    }

    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<User>(context.Get<TicketDbContext>()));
            // Konfiguruj logikę weryfikacji nazw użytkowników
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Konfiguruj logikę weryfikacji haseł
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Konfiguruj ustawienia domyślne blokady użytkownika
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.RegisterTwoFactorProvider("Kod — e-mail", new EmailTokenProvider<User>
            {
                Subject = "Kod zabezpieczeń",
                BodyFormat = "Twój kod zabezpieczający: {0}"
            });
            manager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Skonfiguruj menedżera logowania aplikacji używanego w tej aplikacji.
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
