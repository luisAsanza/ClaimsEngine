using ClaimsEngine.Domain.Common.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClaimsEngine.Infra.Data.Common;

public sealed class SqlExceptionTranslator : IDbExceptionTranslator
{
    public Exception? Translate(DbUpdateException exception)
    {
        // Handle optimistic concurrency exceptions first
        if(exception is DbUpdateConcurrencyException)
        {
            return new ConcurrencyException("The record was modified by another process. Please reload and try again.", exception);
        }

        if(exception?.InnerException is not SqlException sqlException)
        {
            return null;
        }

        var entry = exception.Entries.FirstOrDefault();
        
        return sqlException.Number switch
        {
            2601 or 2627 => MapUniqueViolation(exception, entry, sqlException),
            547 => MapForeignKeyViolation(exception, entry),
            _ => null
        };
    }

    private Exception? MapUniqueViolation(DbUpdateException exception, EntityEntry? entry, SqlException sqlException)
    {
        // Check for "PK_" in the constraint name to identify primary key violations
        bool isPrimaryKeyViolation = sqlException.Message.Contains("PK_", StringComparison.OrdinalIgnoreCase) 
            || sqlException.Message.Contains("PRIMARY KEY", StringComparison.OrdinalIgnoreCase);

        // If it is a PK violation that means it is a system failure that should be investigated by the development team, so we return a generic message to avoid confusion for the client
        if (isPrimaryKeyViolation)
        {
            return null;
        }

        // If it is a unique index violation (e.g. username or email)
        var entityName = entry?.Metadata.ClrType.Name ?? "Unknown Entity";
        var primaryKeyProperties = entry?.Metadata.FindPrimaryKey()?.Properties;
        // Safely extract composite keys
        var primaryKey = (primaryKeyProperties != null &&entry !=null)
            ? string.Join(", ", primaryKeyProperties.Select(p => $"{p.Name}={entry.Property(p.Name).CurrentValue}"))
            : "Unknown Key";
        return new DuplicateResourceException(entityName, primaryKey, exception);
    }

    private Exception MapForeignKeyViolation(DbUpdateException exception, EntityEntry? entry)
    {        
        // If it is a Delete, it means there are dependent records that reference this entity
        if (entry?.State == EntityState.Deleted)
            return new ResourceInUseException("The resource cannot be deleted because it is referenced by other records. Please remove all dependencies before deleting.", exception);  

        // If it is an Add/Update, it means the referenced entity is missing
        return new InvalidReferenceException("The operation violates data integrity constraints. Please ensure all referenced entities exist.", exception); 
    }
}