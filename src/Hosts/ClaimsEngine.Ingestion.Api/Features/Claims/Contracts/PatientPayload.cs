namespace ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;

public sealed record PatientPayload(
    string Name, 
    DateOnly? DateOfBirth, 
    string RelationshipToInsured);
