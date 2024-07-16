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

            var emailSubject = "Verification Code for UniTutor Account";
            var emailMessage = $@"
                <html>
                <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                    }}
                    .email-container {{
                        width: 100%;
                        padding: 20px;
                        background-color: lightblue; /* Changed to blue */
                    }}
                    .email-header {{
                        background-color: #024A7B; /* Changed to blue */
                        color: white;
                        padding: 10px 20px;
                        text-align: center;
                    }}
                    .email-content {{
                        background-color: white;
                        padding: 20px;
                        margin-top: 20px;
                    }}
                    .email-footer {{
                        margin-top: 20px;
                        text-align: center;
                        font-size: 0.9em;
                        color: #777;
                    }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='email-header'>
                            <h1>UniTutor</h1>
                        </div>
                        <div class='email-content'>
                            <p>Dear user,</p>
                            <p>Welcome to UniTutor! Your verification code is:</p>
                            <h2 style='text-align: center; background-color: #f0f0f0; padding: 10px; border-radius: 5px;'>{verificationCode}</h2>
                            <p>Please use this code to verify your account. If you did not request this verification, please ignore this email.</p>
                            <p>If you have any questions or need assistance, please feel free to contact our support team at <a href='mailto:support@unitutor.com'>support@unitutor.com</a>.</p>
                            <p>Best regards,<br>UniTutor Team</p>
                        </div>
                        <div class='email-footer'>
                            <p>This is an automated message, please do not reply directly to this email.</p>
                            <p>If you have any questions, feel free to contact our support team at support@unitutor.com.</p>
                        </div>
                    </div>
                </body>
                </html>";


            await SendEmailAsync(email, emailSubject, emailMessage);
        }
    }
}
