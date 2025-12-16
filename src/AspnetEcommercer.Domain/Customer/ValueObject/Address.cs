namespace AspnetEcommerce.Domain.Customer.ValueObject
{
    public sealed record Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public int Number { get; }

        private Address(string street, string city, string state, string zipCode, int number)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Number = number;

            Validate();
        }

        public static Address Create(string street, string city, string state, string zipCode, int number)
        {
            return new Address(street, city, state, zipCode, number);
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Street))
            {
                throw new Exception("Street is required");
            }

            if (string.IsNullOrWhiteSpace(City))
            {
                throw new Exception("City is required");
            }

            if (string.IsNullOrWhiteSpace(State))
            {
                throw new Exception("State is required");
            }

            if (string.IsNullOrWhiteSpace(ZipCode))
            {
                throw new Exception("ZipCode is required");
            }

            if (Number <= 0)
            {
                throw new Exception("Number is required");
            }
        }

        public override string ToString()
        {
            return $"{Street}, {Number}, {City} - {State}, {ZipCode}";
        }
    }
}