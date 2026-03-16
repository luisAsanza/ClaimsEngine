using Microsoft.EntityFrameworkCore;

namespace ClaimsEngine.Infra.Data.Common;

public interface IDbExceptionTranslator
{
    Exception? Translate(DbUpdateException exception);
}