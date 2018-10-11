using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WatchlistMailManagement.Services
{
    public static class EmailandSMSservice
    {



        /// <summary>
        /// Email Sending Using Gmail Service
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="recipients"></param>
        /// <param name="from"></param>
        /// <returns></returns>

        public static bool SendGmail(string subject, string content, string[] recipients, string from)
        {
            if (recipients == null || recipients.Length == 0)
                throw new ArgumentException("recipients");



            var gmailClient = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings.Get("EmailIdForAlert"), ConfigurationManager.AppSettings.Get("EmailPasswordForAlert"))
                //Credentials = new System.Net.NetworkCredential("alerts@natgashub.com", "Lkjh0962")
            };

            using (var msg = new System.Net.Mail.MailMessage(from, recipients[0], subject, content))
            {
                for (int i = 1; i < recipients.Length; i++)
                    msg.To.Add(recipients[i]);

                // msg.CC.Add("lakhwinder.singh@invertedi.com");
                msg.IsBodyHtml = true;
                try
                {
                    gmailClient.Send(msg);
                    return true;
                }
                catch (Exception ex)
                {
                    // TODO: Handle the exception
                    return false;
                }
            }
        }


        /// <summary>
        /// SMS service using Twilio
        /// </summary>
        /// <param name="to_MobNumber"></param>
        /// <param name="from_MobNumber"></param>
        /// <param name="smscontent"></param>
        /// <param name="mediaurl"></param>
        /// <returns></returns>

        public static bool SendSMS(string to_MobNumber, string from_MobNumber, string smscontent)
        {
            // Find your Account Sid and Auth Token at twilio.com/console
            const string accountSid = "ACa8968569e61567e55d34a45510c0ad82";
            const string authToken = "c79b8822f63ff8d7795fa9ec363ae915";
            try
            {
                //TwilioClient.Init(accountSid, authToken);
                //var to = new PhoneNumber(to_MobNumber);
                //var message = MessageResource.Create(
                //    to,
                //    from: new PhoneNumber(from_MobNumber),
                //    body: smscontent
                //    );
                //Console.WriteLine(message.Sid);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}