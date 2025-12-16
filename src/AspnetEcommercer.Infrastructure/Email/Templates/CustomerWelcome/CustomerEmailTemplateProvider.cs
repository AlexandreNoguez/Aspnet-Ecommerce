using AspnetEcommerce.Application.Customer.Contracts.Email;
using System.Net;

namespace AspnetEcommerce.Infrastructure.Email.Templates.CustomerWelcome
{
    public class CustomerEmailTemplateProvider : ICustomerEmailTemplateProvider
    {
        private readonly string _templatePath;

        public CustomerEmailTemplateProvider()
        {
            _templatePath = Path.Combine(
                AppContext.BaseDirectory,
                "EmailTemplates",
                "CustomerWelcomeTemplate.html");
        }

        public (string Subject, string BodyHtml) BuildWelcomeEmail(
            string customerName,
            string activationLink)
        {
            var subject = "Welcome to AspnetEcommerce";

            if (!File.Exists(_templatePath))
            {
                var fallbackBody =
                    $"Hello {WebUtility.HtmlEncode(customerName)},<br/><br/>" +
                    "Your account has been created successfully and is currently pending activation.<br/><br/>" +
                    $"Please click the following link to activate your account:<br/>" +
                    $"<a href=\"{activationLink}\">{activationLink}</a><br/><br/>" +
                    "Best regards,<br/>AspnetEcommerce Team";

                return (subject, fallbackBody);
            }

            var template = File.ReadAllText(_templatePath);

            template = template
                .Replace("{{CustomerName}}", WebUtility.HtmlEncode(customerName))
                .Replace("{{ActivationLink}}", activationLink);

            return (subject, template);
        }
    }
}
