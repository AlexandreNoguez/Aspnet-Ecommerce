using System;

namespace AspnetEcommerce.Infrastructure.Customer.Models
{
    public class CustomerActivationTokenDbModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Token { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? UsedAt { get; set; }
    }
}
