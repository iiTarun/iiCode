using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace UPRDEngine
{
    /// <summary>
    /// Send an email
    /// </summary>
    /// <param name="to">Message to address</param>
    /// <param name="body">Text of message to send</param>
    /// <param name="subject">Subject line of message</param>
    /// <param name="fromAddress">Message from address</param>
    /// <param name="fromDisplay">Display name for "message from address"</param>    
    /// <param name="attachments">Optional attachments for message</param>
    public class EmailService
    {
        public static void SendEmailAsync(IEnumerable<string> to,
                                 string body,
                                 string subject,
                                 string fromDisplay,
                                 Attachment attachments
                                 )
        {
            try
            {
                if (to == null)
                    throw new ArgumentException("recipients");

                string credentialUser = "Alerts@NatGasHub.com";
                string fromAddress = credentialUser;
                string credentialPassword = "Lkjh0962";
                string host = "smtp.gmail.com";
                int port = 587;

                MailMessage mail = new MailMessage();
                mail.Body = body;
                mail.IsBodyHtml = true;

                foreach (var item in to)
                {
                    mail.To.Add(new MailAddress(item));
                }

                mail.From = new MailAddress(fromAddress, fromDisplay, Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Priority = MailPriority.Normal;

                if (attachments != null)
                {
                    mail.Attachments.Add(attachments);
                }

                SmtpClient smtp = new SmtpClient(host, port);
                smtp.Credentials = new System.Net.NetworkCredential(credentialUser, credentialPassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
