namespace AspnetEcommerce.Application.Contracts.Email.Jobs
{
    public class WelcomeEmailJob
    {
        public Guid CustomerId { get; }
        public string CustomerName { get; }
        public string CustomerEmail { get; }
        public string ActivationLink { get; }

        public WelcomeEmailJob(Guid customerId, string customerName, string customerEmail, string activationLink)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            ActivationLink = activationLink;
        }
    }
}
