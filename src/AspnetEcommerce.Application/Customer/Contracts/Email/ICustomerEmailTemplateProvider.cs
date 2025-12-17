namespace AspnetEcommerce.Application.Customer.Contracts.Email
{
    public interface ICustomerEmailTemplateProvider
    {
        (string Subject, string BodyHtml) BuildWelcomeEmail(
            string customerName,
            string activationLink);
    }
}
