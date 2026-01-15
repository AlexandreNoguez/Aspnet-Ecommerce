namespace AspnetEcommerce.Domain.Product.ValueObject
{
    public sealed record Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
            Validate();
        }

        public static Money Create(decimal amount, string currency)
        {
            return new Money(amount, currency);
        }

        private void Validate()
        {
            if (Amount <= 0)
            {
                throw new Exception("Amount must be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(Currency))
            {
                throw new Exception("Currency is required");
            }

            if (Currency.Length != 3)
            {
                throw new Exception("Currency must be ISO 4217 format");
            }
        }

        public override string ToString()
        {
            return $"{Currency} {Amount:N2}";
        }
    }
}