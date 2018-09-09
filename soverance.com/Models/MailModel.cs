using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace soverance.com.Models
{
    public static class MailModel
    {
        public static string SendContactEmail(MailConfig mailConfig)
        {
            string ReturnResults;

            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add("info@soverance.com");
                message.Subject = mailConfig.Subject;
                message.From = new System.Net.Mail.MailAddress(mailConfig.User);
                message.Body = "First contact has been made on soverance.com!<br /><br />";
                message.Body += "<strong>Contact:</strong> " + mailConfig.ContactName + "<br />";
                message.Body += "<strong>Email:</strong> " + mailConfig.ContactEmail + "<br />";
                message.Body += "<strong>Phone:</strong> " + mailConfig.ContactPhone + "<br />";
                message.Body += "<strong>Subject:</strong> " + mailConfig.Subject + "<br /><br />";
                message.Body += mailConfig.Message;

                message.IsBodyHtml = true;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(mailConfig.Server, mailConfig.Port);
                smtp.Credentials = new System.Net.NetworkCredential(mailConfig.User, mailConfig.Password);
                smtp.EnableSsl = true;
                smtp.Send(message);

                ReturnResults = "Your message was delivered successfully.";
            }
            catch (Exception ex)
            {
                ReturnResults = ex.ToString();
            }

            return ReturnResults;
        }
        
    }
}
