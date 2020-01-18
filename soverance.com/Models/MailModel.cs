using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace soverance.com.Models
{
    public static class MailModel
    {
        public static string SendContactEmail(MailConfig_Contact mailConfig)
        {
            string ReturnResults;

            try
            {
                bool bIsSubjectClean = FilterTest(mailConfig.Subject);
                bool bIsMessageClean = FilterTest(mailConfig.Message);

                if (bIsSubjectClean && bIsMessageClean)
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
                else
                {
                    ReturnResults = "Your message failed to process because it included restricted content.";
                }
            }
            catch (Exception ex)
            {
                ReturnResults = ex.ToString();
            }

            return ReturnResults;
        }

        // Soverance blocks all form submissions containing certain undesireable words
        public static bool WordFilterCheck(string input)
        {
            Regex Filter = new Regex("(porn|sex|nude|gay|lesbian|viagra|slut|sale|buy|fuck|ass|shit|cunt|pussy|dick|penis|tits|lesbo|fag|faggot|lgbt)");
            return Filter.IsMatch(input);
        }

        // Soverance blocks any form submission containing a URL
        public static bool UrlFilterCheck(string input)
        {
            Regex Filter = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$");
            return Filter.IsMatch(input);
        }

        // Soverance blocks any form submission that contains a character outside the Latin alphabet
        public static bool LanguageFilterCheck(string input)
        {
            Regex Filter = new Regex("^[a-zA-Z0-9 ]*$");
            return Filter.IsMatch(input);
        }

        // Test a string for clean input.  Returns true if the test is passed, false if the test is failed.
        public static bool FilterTest(string input)
        {
            bool bContainsBadWords = WordFilterCheck(input);
            bool bContainsUrls = UrlFilterCheck(input);
            bool bIsLatin = LanguageFilterCheck(input);

            if (!bContainsBadWords && !bContainsUrls && bIsLatin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string SendClientLink(MailConfig_Send mailConfig)
        {
            string ReturnResults;

            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(mailConfig.Recipient);
                message.Subject = mailConfig.Subject;
                message.From = new System.Net.Mail.MailAddress(mailConfig.User);
                message.Body = mailConfig.Message;
                message.IsBodyHtml = true;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(mailConfig.Server, mailConfig.Port);
                smtp.Credentials = new System.Net.NetworkCredential(mailConfig.User, mailConfig.Password);
                smtp.EnableSsl = true;
                smtp.Send(message);

                ReturnResults = "Your message was sent successfully.";
            }
            catch (Exception ex)
            {
                ReturnResults = ex.ToString();
            }

            return ReturnResults;
        }
    }
}
