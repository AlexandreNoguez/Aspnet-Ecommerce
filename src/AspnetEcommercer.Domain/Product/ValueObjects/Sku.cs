namespace AspnetEcommerce.Domain.Product.ValueObject
{
    public sealed record Sku
    {
        public string Value { get; }

        private Sku(string value)
        {
            Value = value;
            Validate();
        }

        public static Sku Create(string value)
        {
            return new Sku(value);
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                throw new Exception("SKU is required");
            }

            if (Value.Length > 50)
            {
                throw new Exception("SKU must be 50 characters or less");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
