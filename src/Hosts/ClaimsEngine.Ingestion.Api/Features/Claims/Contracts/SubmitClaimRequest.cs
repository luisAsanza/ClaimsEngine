namespace ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;

public sealed record SubmitClaimRequest(
    Guid CorrelationId,
    string ClaimNumber,
    string SubscriberId,
    string PayerId,
    string ProviderNpi,
    PatientPayload Patient,
    InsuredPayload Insured,
    List<LineItemPayload> LineItems);
