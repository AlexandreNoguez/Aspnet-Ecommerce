namespace AspnetEcommerce.Application.Customer.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message ?? throw new ArgumentNullException(nameof(message))) { }

        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}