using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Entities;

namespace UserStore.BLL.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(Request Request)
        {
            MailAddress from = new MailAddress("somemail@gmail.com", "Tom");
            MailAddress to = new MailAddress("somemail@yandex.ru");
            MailMessage mail = new MailMessage(from, to);
            mail.Subject = "Тест";
            mail.Body = "Письмо-тест 2 работы smtp-клиента";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("somemail@gmail.com", "mypassword");
            await smtp.SendMailAsync(mail);
        }
    }
}
