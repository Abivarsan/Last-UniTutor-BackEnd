using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UniTutor.Interface;

namespace UniTutor.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;

        }

        private SmtpClient ConfigureSmtpClient()
        {
            var smtpHost = _config["EmailConfiguration:Host"];
            var smtpPort = int.Parse(_config["EmailConfiguration:Port"]);
            var smtpUsername = _config["EmailConfiguration:Username"];
            var smtpPassword = _config["EmailConfiguration:Password"];

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpPort.ToString()) ||
                string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
            {
                throw new InvalidOperationException("Email configuration settings are not properly set.");
            }

            var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            return smtpClient;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            var smtpClient = ConfigureSmtpClient();
            var smtpFrom = _config["EmailConfiguration:From"];

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpFrom),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(recipient);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }

        public async Task SendVerificationCodeAsync(string email, string verificationCode)
        {
            if (string.IsNullOrEmpty(verificationCode))
            {
                throw new ArgumentNullException(nameof(verificationCode), "Verification code cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
            }

            var subject = "Password Reset Verification Code";
            var body = $"Your verification code is: {verificationCode}";

            await SendEmailAsync(email, subject, body);
        }
    }
}
