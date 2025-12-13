using AspnetEcommercer.Domain.Customer.Entity;
using AspnetEcommercer.Domain.Customer.ValueObject;

namespace AspnetEcommercer.Domain.Customer.Factory
{
    public class CustomerFactory
    {
        public static CustomerEntity CreateNewCustomer(string name, Address address)
        {
            return new CustomerEntity(Guid.NewGuid(), name, address, false, 0);
        }
    }
}
