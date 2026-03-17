namespace ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;

public sealed record LineItemPayload(decimal Amount, string Description, int Quantity);
