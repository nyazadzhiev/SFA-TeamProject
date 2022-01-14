using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.DTO.Models;
using WorkforceManagementAPI.BLL.Contracts;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using MailKit.Net.Smtp;

namespace WorkforceManagementAPI.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppSettings _appSettings;

        public NotificationService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task Send(string to, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_appSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = message };

            var client = new SmtpClient();

            client.ServerCertificateValidationCallback = (object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors) => true;

            await client.ConnectAsync(_appSettings.Server, _appSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(new NetworkCredential(_appSettings.SenderEmail, _appSettings.Password));
            await client.SendAsync(email);
            await client.DisconnectAsync(true);

            client.Dispose();
        }
    }
}
