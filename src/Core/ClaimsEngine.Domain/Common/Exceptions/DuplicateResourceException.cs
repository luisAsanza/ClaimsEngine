namespace ClaimsEngine.Domain.Common.Exceptions;

public class DuplicateResourceException : DomainException
{
    public string EntityName { get; set; }
    public object EntityKey { get; set; }

    public DuplicateResourceException(string entityName, object entityKey) : 
        base($"Entity '{entityName}' with key '{entityKey}' already exists.")
    {
        EntityName = entityName;
        EntityKey = entityKey;
    }

    public DuplicateResourceException(string entityName, object entityKey, Exception exception) : 
        base($"Entity '{entityName}' with key '{entityKey}' already exists.", exception)
    {
        EntityName = entityName;
        EntityKey = entityKey;
    }
}