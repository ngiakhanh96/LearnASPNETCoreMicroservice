using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettingsOptions EmailSettingsOptions { get; }

        private readonly ILogger<EmailService> _logger;

        private readonly SendGridClient _client;

        public EmailService(IOptions<EmailSettingsOptions> emailSettings, ILogger<EmailService> logger)
        {
            EmailSettingsOptions = emailSettings.Value;
            _logger = logger;
            _client = new SendGridClient(EmailSettingsOptions.ApiKey);
        }

        public async Task<bool> SendEmail(Email email)
        {
            var sendGridMessage = MailHelper.CreateSingleEmail(
                new EmailAddress(EmailSettingsOptions.FromAddress, EmailSettingsOptions.FromName),
                new EmailAddress(email.To),
                email.Subject,
                email.Body,
                email.Body);

            var response = await _client.SendEmailAsync(sendGridMessage);
            _logger.LogInformation("Email sent");

            if (response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK)
            {
                return true;
            }
            _logger.LogError("Email sending failed.");

            return false;
        }
    }
}
