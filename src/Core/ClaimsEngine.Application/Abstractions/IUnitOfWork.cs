using ClaimsEngine.Domain.SeedWork;

namespace ClaimsEngine.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}