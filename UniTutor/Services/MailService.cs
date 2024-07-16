using System.Net.Mail;
using System.Net;
using UniTutor.Interface;

namespace UniTutor.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var from = _configuration["Smtp:From"];
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"]);
            var smtpUsername = _configuration["Smtp:Username"];
            var smtpPassword = _configuration["Smtp:Password"];

                var mailMessage = new MailMessage(from, toEmail, subject, body);
            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            smtpClient.Send(mailMessage);
        }
    }
}