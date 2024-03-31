using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using ServicesHealthCheck.Business.Notifications.EMailService.Abstract;
using ServicesHealthCheck.Dtos.MailDtos;

namespace ServicesHealthCheck.Business.Notifications.EMailService
{
    public class MailService : IMailService
    {
        public async Task SendEmailAsync(MailDto mail, CancellationToken cancellationToken)
        {
            MimeMessage email = new();
            email.From.Add(new MailboxAddress("HealthCheck", mail.FromMail));

            email.To.Add(new MailboxAddress("User", mail.ToEmail));

            email.Subject = mail.Subject;

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = mail.Body
            };

            email.Body = bodyBuilder.ToMessageBody();
            email.Body.Headers.Add("Content-Type", "text/html; charset=UTF-8");

            using SmtpClient smtp = new();
            await smtp.ConnectAsync("smtp.gmail.com", 587, cancellationToken: cancellationToken);
            await smtp.AuthenticateAsync(mail.FromMail, "yylvfposgsbqqwmw", cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
