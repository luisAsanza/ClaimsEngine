namespace ClaimsEngine.Domain.Common.Exceptions;

public class InvalidReferenceException : DomainException
{
    
    public InvalidReferenceException(string message) : base(message)
    {
    }

    public InvalidReferenceException(string message, Exception exception) : base(message, exception)
    {
    }
}