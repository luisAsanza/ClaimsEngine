namespace ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;

public record ClaimAcceptedResponse(Guid ClaimId, Guid CorrelationId, string Status);
