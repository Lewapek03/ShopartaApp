using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Shoparta.Services
{
    public interface IEmailSender
    {        
        Task SendEmailAsync(string email, string subject, string htmlMessage);

    }
}
