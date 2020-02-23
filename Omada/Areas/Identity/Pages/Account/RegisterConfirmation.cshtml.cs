using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Omada.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;

namespace Omada.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<OmadaUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<OmadaUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { area = "Identity", userId = userId, code = code },
            protocol: Request.Scheme);

            Email = email;
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("Admin@omada.com");
                mail.To.Add(Email);
                mail.Subject = "Confirm Email";
                mail.Body = $"Thank you for register. Please confirm your account by clicking following link "
                            + EmailConfirmationUrl;
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("Mczajczej@gmail.com", "password");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return Page();
        }
    }
}
