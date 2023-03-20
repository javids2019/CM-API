using System;
using System.ComponentModel;
using System.Text;
using System.Web.Mail;

namespace MohsyWebApi.common
{
    public class SendClientMail
    {
        private static bool _isSucess = true;
        public static bool SendMailContactUs(string toMail, string subject, string body)
        {
            try
            {
                var adminCCEmail = Utility.GetConfigValue("AdminCCEmailId");
                var adminFromEmail = Utility.GetConfigValue("FromEmailId");
                var hostName = Utility.GetConfigValue("HostName");
                var portno = Convert.ToInt32(Utility.GetConfigValue("PortNumber"));
                var fromEmailPassword = Utility.GetConfigValue("FromEmailPassword");

                var message = new MailMessage();
                message.To = "maaz@peoplematrimonial.com";
                message.Cc = adminCCEmail;
                message.Subject = subject;
                message.From = adminFromEmail;
                message.Body = body;
                message.BodyFormat = MailFormat.Html;
                message.Priority = MailPriority.High;
                SmtpMail.SmtpServer = hostName;
                //message.IsBodyHtml = true;
                SmtpMail.Send(message);
            }
            catch (Exception ex)
            {
                _isSucess = false;
            }
            return _isSucess;
        }

        public static bool SendMail(string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.To = "maaz@classymatrimony.com;javids.net@gmail.com;maaz@peoplematrimonial.com";
                message.Subject = subject;
                message.From = "classyvows123@gmail.com";
                message.Body = body;
                message.BodyFormat = MailFormat.Html;
                message.Priority = MailPriority.High;
                SmtpMail.SmtpServer = "relay-hosting.secureserver.net";
                SmtpMail.Send(message);
            }
            catch (Exception ex)
            {
                _isSucess = false;
            }
            return _isSucess;
        }

        public static string SendTestMail(string subject, string body, string toEmail, string fromEmail, string ccEmail, string hostName)
        {
            var theBody = new StringBuilder();
            try
            {
                var message = new MailMessage();
                message.To = toEmail;
                //  message.To = "maaz@peoplematrimonial.com"; 
                message.Cc = ccEmail;
                message.Subject = subject;
                message.From = fromEmail;
                message.Body = body;
                message.BodyFormat = MailFormat.Html;
                message.Priority = MailPriority.High;
                SmtpMail.SmtpServer = hostName;
                SmtpMail.Send(message);
            }
            catch (Exception ex)
            {

                theBody.Append("Message: " + ex.Message + "\n");
                theBody.Append("StackTrace: " + ex.StackTrace + "\n");
                theBody.Append("InnerException: " + ex.InnerException + "\n");

            }
            return theBody.ToString();
        }


        public static bool SendDevMail(string subject, string body)
        {
            try
            {
                var message = new System.Net.Mail.MailMessage();
                message.To.Add("javids.net@gmail.com");
                message.Subject = subject;
                message.From = new System.Net.Mail.MailAddress("javidbece@gmail.com");
                message.Bcc.Add(new System.Net.Mail.MailAddress("javidbece@gmail.com"));
                message.Body = body;
                message.Priority = System.Net.Mail.MailPriority.High;
                message.IsBodyHtml = true;
                var smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("javidbece@gmail.com", "Thasneem-12345")
                };
                smtp.SendCompleted += delegate (object s, AsyncCompletedEventArgs b)
                {
                    smtp.Dispose();
                    message.Dispose();
                };
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                LoggingTextServices.LogError(ex);
                _isSucess = false;
            }
            return _isSucess;
        }

    }
}