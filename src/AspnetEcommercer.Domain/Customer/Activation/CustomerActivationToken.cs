namespace AspnetEcommerce.Domain.Customer.Activation
{
    public class CustomerActivationToken
    {
        private CustomerActivationToken() { }

        public CustomerActivationToken(
            Guid id,
            Guid customerId,
            string token,
            DateTimeOffset createdAt,
            DateTimeOffset expiresAt,
            DateTimeOffset? usedAt = null)
        {
            Id = id;
            CustomerId = customerId;
            Token = token ?? throw new ArgumentNullException(nameof(token));
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
            UsedAt = usedAt;

            Validate();
        }

        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Token { get; private set; } = null!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }
        public DateTimeOffset? UsedAt { get; private set; }

        public bool IsUsed => UsedAt.HasValue;

        public bool IsExpired(DateTimeOffset? now = null)
            => (now ?? DateTimeOffset.UtcNow) > ExpiresAt;

        public static CustomerActivationToken CreateNew(
            Guid customerId,
            string token,
            DateTimeOffset expiresAt)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("CustomerId is required.", nameof(customerId));

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token is required.", nameof(token));

            var now = DateTimeOffset.UtcNow;

            if (expiresAt <= now)
                throw new ArgumentException("ExpiresAt must be in the future.", nameof(expiresAt));

            return new CustomerActivationToken(
                id: Guid.NewGuid(),
                customerId: customerId,
                token: token,
                createdAt: now,
                expiresAt: expiresAt);
        }

        public void MarkAsUsed()
        {
            if (IsUsed)
                throw new InvalidOperationException("Activation token is already used.");

            UsedAt = DateTimeOffset.UtcNow;
        }

        public void EnsureIsValid()
        {
            if (IsUsed)
                throw new InvalidOperationException("Activation token already used.");

            if (IsExpired())
                throw new InvalidOperationException("Activation token is expired.");
        }

        private void Validate()
        {
            if (Id == Guid.Empty)
                throw new InvalidOperationException("Id is required.");

            if (CustomerId == Guid.Empty)
                throw new InvalidOperationException("CustomerId is required.");

            if (string.IsNullOrWhiteSpace(Token))
                throw new InvalidOperationException("Token is required.");
        }
    }
}
