namespace AspnetEcommerce.Domain.Product.ValueObject
{
    public sealed record ProductName
    {
        public string Value { get; }

        private ProductName(string value)
        {
            Value = value;
            Validate();
        }

        public static ProductName Create(string value)
        {
            return new ProductName(value);
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                throw new Exception("Product name is required");
            }

            if (Value.Length > 200)
            {
                throw new Exception("Product name must be 200 characters or less");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
