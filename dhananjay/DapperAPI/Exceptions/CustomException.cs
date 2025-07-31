namespace DapperAPI.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base(){}
    }
    public class InvalidDetailsException : Exception
    {
        public InvalidDetailsException() : base("Details missing or invalid ") { }
    }
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid credentials provided.") { }

    }

    public class UnauthorizedAccessAppException : Exception
    {
        public UnauthorizedAccessAppException() : base("Access is denied.") { }
    }

    public class ResourceConflictException : Exception
    {
        public ResourceConflictException() : base("The Data already exists or conflicts with another.") { }
    }

    

}
