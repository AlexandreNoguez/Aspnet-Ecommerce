namespace AspnetEcommerce.Application.Customer.Contracts.Links;

public interface IActivationLinkBuilder
{
    string Build(string token);
}