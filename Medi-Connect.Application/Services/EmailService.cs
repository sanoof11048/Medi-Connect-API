using Medi_Connect.Application.Interfaces.ISerives;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_config["Smtp:Host"])
                {
                    Port = int.Parse(_config["Smtp:Port"]),
                    Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["Smtp:From"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
                throw;
            }
        }

    }

}
