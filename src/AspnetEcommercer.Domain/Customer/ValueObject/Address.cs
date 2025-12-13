namespace AspnetEcommercer.Domain.Customer.ValueObject
{
    public class Address(string street, string city, string state, string zipCode, int number)
    {
        public string Street { get; set; } = street;
        public string City { get; set; } = city;
        public string State { get; set; } = state;
        public string ZipCode { get; set; } = zipCode;
        public int Number { get; set; } = number;

        public void Validate()
        {
            if (this.Street.Length == 0)
            {
                throw new Exception("Street is required");
            }

            if (this.City.Length == 0)
            {
                throw new Exception("City is required");
            }

            if (this.State.Length == 0)
            {
                throw new Exception("State is required");
            }

            if (this.ZipCode.Length == 0)
            {
                throw new Exception("ZipCode is required");
            }

            if (this.Number <= 0)
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
