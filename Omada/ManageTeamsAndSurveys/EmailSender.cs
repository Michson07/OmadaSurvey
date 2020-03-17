using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

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
                    Console.WriteLine("Blad" + ex);
                    return false;
                }
            }
        }
    }
}
