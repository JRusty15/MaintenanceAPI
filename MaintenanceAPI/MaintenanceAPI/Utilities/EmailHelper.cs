using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MaintenanceAPI.Utilities
{
    public static class EmailHelper
    {
        private static readonly string m_APIKEY = ConfigurationManager.AppSettings["SendGridAPIKey"].ToString();
        private static readonly string m_DEFAULT_FROM_EMAIL = ConfigurationManager.AppSettings["DefaultFromEmail"].ToString();

        public static bool SendEmail(string to, string subject, string body, string from = null)
        {
            var success = false;

            var mailMessage = new SendGridAPIClient(m_APIKEY);

            Email fromEmail = new Email(from == null ? m_DEFAULT_FROM_EMAIL : from);
            Email toEmail = new Email(to);
            Content content = new Content("text/plain", body);
            Mail mail = new Mail(fromEmail, subject, toEmail, content);

            try
            {
                mailMessage.client.mail.send.post(requestBody: mail.Get());
                success = true;
            }
            catch(Exception ex)
            {
                success = false;
            }
            return success;
        }
    }
}