namespace AspnetEcommerce.Domain.Product.ValueObject
{
    public sealed record ProductDescription
    {
        public string Value { get; }

        private ProductDescription(string value)
        {
            Value = value;
            Validate();
        }

        public static ProductDescription Create(string value)
        {
            return new ProductDescription(value);
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                throw new Exception("Product description is required");
            }

            if (Value.Length > 2000)
            {
                throw new Exception("Product description must be 2000 characters or less");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}