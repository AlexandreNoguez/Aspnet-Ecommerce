using AspnetEcommerce.Domain.Customer.Entity;
using AspnetEcommerce.Domain.Customer.ValueObject;

namespace AspnetEcommerce.Domain.Customer.Factory
{
    public class CustomerFactory
    {
        public static CustomerEntity CreateNewCustomer(string name, string email, Address address)
        {
            return new CustomerEntity(Guid.NewGuid(), name, email, address, false, 0);
        }
    }
}
