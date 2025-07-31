namespace Dapper_Api_With_Token_Authentication.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
