using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Shoparta.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SendGridOptions _sendGridOptions;

        public EmailSender(IOptions<SendGridOptions> sendGridOptions)
        {
            _sendGridOptions = sendGridOptions.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var client = new SendGridClient(_sendGridOptions.ApiKey);
            var from = new EmailAddress("shoparta@wp.pl", "Shoparta");
            var to = new EmailAddress(toEmail);
            var plainTextContent = message;
            var htmlContent = $"{message}";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }

    public class SendGridOptions
    {
        public string ApiKey { get; set; }
    }

}
