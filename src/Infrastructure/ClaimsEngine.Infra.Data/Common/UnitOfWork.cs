using ClaimsEngine.Application.Abstractions;
using ClaimsEngine.Infra.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClaimsEngine.Infra.Data.Common;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ClaimDbContext _dbContext;
    private readonly IDbExceptionTranslator _exceptionTranslator;

    public UnitOfWork(ClaimDbContext dbContext, IDbExceptionTranslator exceptionTranslator)
    {
        _dbContext = dbContext;
        _exceptionTranslator = exceptionTranslator;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateException dbUpdateException)
        {
            var translatedException = _exceptionTranslator.Translate(dbUpdateException);
            throw translatedException ?? dbUpdateException;
        }
    }
}