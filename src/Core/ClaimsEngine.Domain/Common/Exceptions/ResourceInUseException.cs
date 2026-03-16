namespace ClaimsEngine.Domain.Common.Exceptions;

public class ResourceInUseException : DomainException
{
    
    public ResourceInUseException(string message) : base(message)
    {
    }

    public ResourceInUseException(string message, Exception exception) : base(message, exception)
    {
    }
}