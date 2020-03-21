using Microsoft.Extensions.Configuration;
using Omada.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Omada.Pages.ManageUser;

namespace Omada.ManageTeamsAndSurveys
{
    public class EmailSender
    {
        string SenderEmail;
        string SenderPassword;

        public EmailSender()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            SenderEmail = configurationRoot.GetSection("EmailSender").GetSection("Mail").Value;
            SenderPassword = configurationRoot.GetSection("EmailSender").GetSection("Password").Value;
        }

        public bool SendEmail(string receiverMail, string subject, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(SenderEmail);
                mail.To.Add(receiverMail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                try
                {
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        return true;
                    }                
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                    return false;
                }
            }
        }
        public void InformAboutSurvey()
        {
            UserDataNoEntity userData = new UserDataNoEntity();
            var users = userData.GetAllUsers();
            string subject = "Take a survey this week";
            DateTime date = DateTime.Now;
            if (date.DayOfWeek == DayOfWeek.Thursday)
            {
                foreach (OmadaUser user in users)
                {
                    string body = $"Hello {user.Email}\n" +
                        $"You can take a survey for this week.";
                    if(SendEmail(user.Email, subject, body))
                    {
                        Console.WriteLine("Email sent");
                    }
                }
            }
        }
    }
}
