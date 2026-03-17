namespace ClaimsEngine.Application.Features.Claims.DTOs;

public sealed record InsuredDto(
    string Name,
    DateOnly? DateOfBirth
);
