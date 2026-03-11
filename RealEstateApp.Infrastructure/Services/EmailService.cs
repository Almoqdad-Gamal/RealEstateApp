using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendPasswordResetEmailAsync(string toEmail, string resetToken)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var templatePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Templates",
                "PasswordResetEmail.html"
            );
            var template = await File.ReadAllTextAsync(templatePath);

            // Switch the placeholder with the link 
            var resetLink = $"{emailSettings["AppUrl"]}/reset-password?token={resetToken}";
            var emailBody = template.Replace("{{ResetLink}}", resetLink);

            var smtpClient = new SmtpClient(emailSettings["SmtpHost"])
            {
                Port = int.Parse(emailSettings["SmtpPort"]!),
                Credentials = new NetworkCredential(
                    emailSettings["SenderEmail"],
                    emailSettings["SenderPassword"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings["SenderEmail"]!),
                Subject = "Reset Your Password",
                Body = emailBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}