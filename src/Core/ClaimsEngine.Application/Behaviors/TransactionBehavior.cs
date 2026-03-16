using ClaimsEngine.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsEngine.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(IUnitOfWork unitOfWork, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var commandName = typeof(TRequest).Name;
        _logger.LogInformation("Starting transaction for {CommandName}", commandName);

        // 1. Execute the Handler
        var response = await next();

        // 2. Commit the transaction
        var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Transaction committed for {CommandName}. Rows affected: {RowsAffected}", 
            commandName, rowsAffected);

        return response;
    }
}