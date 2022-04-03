using System.Net.Mail;
using EducationManagementSystem.ViewModels;

namespace EducationManagementSystem.Services
{
    public class Email
    {
        public Boolean sendMail(EmailViewModel email)
        {
            try
            {
                MailMessage mm = new MailMessage()
                {
                    From = new MailAddress("tpass3506@gmail.com")
                };
                foreach (string To in email.To)
                {
                    mm.To.Add(new MailAddress(To));
                }
                mm.Subject = email.Subject;
                mm.Body = email.Body;
                mm.IsBodyHtml = email.isHTML;
                if (email.Attachment != null)
                    mm.Attachments.Add(new Attachment(email.Attachment));

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("tpass3506@gmail.com", "Mihir@1322"); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mm);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}