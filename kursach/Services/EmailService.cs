using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Services
{
    internal class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _name = "Риелторское агенство Heaven";
        private readonly string _email = "telitsa.anna2005@gmail.com";
        private readonly string _password = "avre dvko vfah fsqv";
        
        public MailMessage CreateMessage(string toEmail, string emailSubject, string emailBody)
        {
            MailAddress from = new MailAddress(_email, _name);
            MailAddress to = new MailAddress(toEmail);
            MailMessage message = new MailMessage(from, to);

            message.Subject = emailSubject;
            message.Body = emailBody;
            message.IsBodyHtml = true;

            return message;
        }

        public void SendMessage(MailMessage message)
        {
            SmtpClient smtp = new SmtpClient(_smtpServer, _smtpPort);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_email, _password);
            smtp.EnableSsl = true;
            smtp.Send(message);
        }

        public void Send(string toEmail, string emailSubject, string emailBody)
        {
            SendMessage(CreateMessage(toEmail, emailSubject, emailBody));
        }
    }

}