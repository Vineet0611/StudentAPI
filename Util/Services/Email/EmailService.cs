using System.Net.Mail;

namespace Util.Services.Email
{
    public class EmailService : IEmailService
    {
        public void SendMail(string EmailDisplayName, string Subject, string From, string To, string HTMLContent)
        {
            SendMessage(To, Subject, HTMLContent);
        }

        private MailMessage SendMessage(string tos, string Subject, string HTMLContent)
        {
            try
            {
                
                MailMessage mail = new MailMessage();
                mail.To.Add(tos);
                mail.From = new MailAddress(EmailConfig.MailUserName);
                mail.Subject = Subject;
                mail.Body = HTMLContent;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(EmailConfig.HostName, EmailConfig.Port);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EmailConfig.MailUserName, EmailConfig.Password);
                smtp.Send(mail);

                return mail;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

                return new MailMessage();
            }
        }
    }
}
