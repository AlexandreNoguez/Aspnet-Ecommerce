using AspnetEcommerce.Application.Customer.Contracts.Links;

namespace AspnetEcommerce.WebApi.Infra.Links
{
    public class ConfigurationActivationLinkBuilder : IActivationLinkBuilder
    {
        private readonly string _baseUrl;

        public ConfigurationActivationLinkBuilder(IConfiguration configuration)
        {
            _baseUrl = configuration.GetValue<string>("ApiSettings:CustomerActivateUrl")
                ?? throw new ArgumentNullException("ActivationBaseUrl configuration is missing.");

        }

        public string Build(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));
            var activationLink = $"{_baseUrl}?token={Uri.EscapeDataString(token)}";

            return activationLink.ToString();
        }
    }
}
