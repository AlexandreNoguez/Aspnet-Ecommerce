using AspnetEcommerce.Domain.Customer.Activation;
using AspnetEcommerce.Infrastructure.Customer.Models;

namespace AspnetEcommerce.Infrastructure.Customer.Mappers
{
    public static class CustomerActivationTokenMapper
    {
        public static CustomerActivationTokenDbModel ToDbModel(CustomerActivationToken entity)
        {
            return new CustomerActivationTokenDbModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                Token = entity.Token,
                CreatedAt = entity.CreatedAt,
                ExpiresAt = entity.ExpiresAt,
                UsedAt = entity.UsedAt
            };
        }

        public static CustomerActivationToken ToDomain(CustomerActivationTokenDbModel model)
        {
            return new CustomerActivationToken(
                id: model.Id,
                customerId: model.CustomerId,
                token: model.Token,
                createdAt: model.CreatedAt,
                expiresAt: model.ExpiresAt,
                usedAt: model.UsedAt
            );
        }
    }
}
