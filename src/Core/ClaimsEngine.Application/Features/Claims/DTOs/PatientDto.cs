using ClaimsEngine.Domain.Aggregates.ClaimAggregate;

namespace ClaimsEngine.Application.Features.Claims.DTOs;

public sealed record PatientDto(
    string Name,
    DateOnly? DateOfBirth,
    RelationshipToInsured RelationshipToInsured
);
