namespace Dapper_Api_With_Token_Authentication.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
