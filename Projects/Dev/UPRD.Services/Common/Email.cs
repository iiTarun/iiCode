using System;
using System.Collections;
using System.Net.Mail;
using UPRD.Data.Repositories;
using UPRD.Model;

namespace UPRD.Common
{
    public class Email
    {
         IEmailTemplateRepository _serviceEmailTemplate;
         IEmailQueueRepository _serviceEmailQueue;
         IApplicationLogRepository _serviceAppLog;
        public Email(IEmailTemplateRepository serviceEmailTemplate,
        IEmailQueueRepository serviceEmailQueue,
        IApplicationLogRepository serviceAppLog)
        {
            this._serviceAppLog = serviceAppLog;
            this._serviceEmailQueue = serviceEmailQueue;
            this._serviceEmailTemplate = serviceEmailTemplate;
        }
        public void SendEmail(string to, string cc, string bcc, string subject, string emailBody)
        {
            string UserID = "noreply@invertedi.com";
            string Pass = "Monday09";
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                MailMessage message = new MailMessage();

                message.To.Add(to);
                if (!String.IsNullOrEmpty(cc))
                {
                    message.Bcc.Add(cc);
                }

                if (!String.IsNullOrEmpty(bcc))
                {
                    message.Bcc.Add(bcc);
                }

                message.Priority = MailPriority.High;
                message.IsBodyHtml = true;

                message.Subject = subject;
                message.Body = emailBody.Replace("\n", "<br>");
                message.From = new MailAddress("noreply@invertedi.com");
                client.Credentials = new System.Net.NetworkCredential(UserID, Pass);
                client.EnableSsl = true;

                client.Send(message);
            }
            catch (SmtpFailedRecipientException ex)
            {
                AppLogManager("Sending Email", "Error", ex.Message);
            }
            catch (SmtpException ex)
            {
                AppLogManager("Sending Email", "Error", ex.Message);
            }
            catch (Exception ex)
            {
                AppLogManager("Sending Email", "Error", ex.Message);
            }
        }
        public void SendEmail(string[] to, string[] cc, string[] bcc, int emailTemplateID, Hashtable param, string userID)
        {
            try
            {
                EmailTemplate emailTemplate = _serviceEmailTemplate.GetById(emailTemplateID);
                param.Add("%TemplateName", emailTemplate.Description);

                string subject = emailTemplate.Subject;
                string emailBody = emailTemplate.Body;

                //replace the tags with actual values
                if (param != null)
                {
                    foreach (DictionaryEntry p in param)
                    {
                        if (emailTemplate.Subject.Contains(p.Key.ToString()))
                            subject = subject.Replace(p.Key.ToString(), p.Value.ToString());

                        if (emailTemplate.Body.Contains(p.Key.ToString()))
                            emailBody = emailBody.Replace(p.Key.ToString(), p.Value.ToString());
                    }
                }
                else
                {
                    subject = emailTemplate.Subject;
                    emailBody = emailTemplate.Body;
                }

                //save the email in the queue table to be sent later on
                if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(emailBody))
                {
                    EmailQueue emailQueue = new EmailQueue();

                    if (to != null)
                    {
                        if (to.Length > 0)
                        {
                            foreach (string toAddress in to)
                            {
                                emailQueue.Recipient = emailQueue.Recipient + toAddress + ",";
                            }

                            //remove the last comma
                            emailQueue.Recipient = emailQueue.Recipient.Remove(emailQueue.Recipient.LastIndexOf(","));
                        }
                    }

                    if (cc != null)
                    {
                        if (cc.Length > 0)
                        {
                            foreach (string ccAddress in cc)
                            {
                                emailQueue.CC = ccAddress + ",";
                            }

                            //remove the last comma
                            emailQueue.CC = emailQueue.CC.Remove(emailQueue.CC.LastIndexOf(","));
                        }
                    }
                    else
                        emailQueue.CC = "";

                    if (bcc != null)
                    {
                        if (bcc.Length > 0)
                        {
                            foreach (string bccAddress in cc)
                            {
                                emailQueue.Bcc = bccAddress + ",";
                            }

                            //remove the last comma
                            emailQueue.Bcc = emailQueue.Bcc.Remove(emailQueue.Bcc.LastIndexOf(","));
                        }
                    }
                    else
                        emailQueue.Bcc = "";

                    DateTime currentDateTime = DateTime.Now;
                    emailQueue.ToUserID = userID;
                    emailQueue.Subject = subject;
                    emailQueue.Email = emailBody;
                    emailQueue.IsSent = false;

                    emailQueue.CreatedDate = currentDateTime;
                    emailQueue.SentDate = currentDateTime;
                    try
                    {
                        if (!emailQueue.IsSent)
                        {
                            SendEmail(emailQueue.Recipient, emailQueue.CC.ToString(), emailQueue.Bcc, emailQueue.Subject, emailQueue.Email);
                            emailQueue.IsSent = true;
                            emailQueue.SentDate = DateTime.Now;
                            _serviceEmailQueue.Add(emailQueue);
                            _serviceEmailQueue.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationLog log = new ApplicationLog();
                        log.CreatedDate = DateTime.UtcNow;
                        log.Source = ex.Source;
                        log.Type = "Error in Sending Email.";
                        log.Description = ex.Message;
                        _serviceAppLog.Add(log);
                        _serviceAppLog.Save();
                    }

                }
            }
            catch (Exception ex)
            {
                ApplicationLog log = new ApplicationLog();
                log.CreatedDate = DateTime.UtcNow;
                log.Source = ex.Source;
                log.Type = "Error in Sending Email.";
                log.Description = ex.Message;
                _serviceAppLog.Add(log);
                _serviceAppLog.Save();
            }
        }
        private void AppLogManager(string source, string type, string description)
        {
            ApplicationLog log = new ApplicationLog();
            log.CreatedDate = DateTime.UtcNow;
            log.Source = source;
            log.Type = type;
            log.Description = description;
            _serviceAppLog.Add(log);
            _serviceAppLog.Save();
        }
    }
}
