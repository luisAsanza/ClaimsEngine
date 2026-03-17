using FluentValidation;
using ClaimsEngine.Domain.Aggregates.ClaimAggregate;
using ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;

namespace ClaimsEngine.Ingestion.Api.Features.Claims.Validators;

public sealed class PatientPayloadValidator : AbstractValidator<PatientPayload>
{
    public PatientPayloadValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Patient name is required.")
            .MaximumLength(200);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Patient date of birth is required.")
            .LessThan(GetLatestPossibleDate(timeProvider)).WithMessage("Date of birth must be in the past.");

        // Make message dynamic based on enum values
        var relationshipValues = string.Join(", ", Enum.GetNames<RelationshipToInsured>());
        RuleFor(x => x.RelationshipToInsured)
            .NotEmpty().WithMessage("Relationship to insured is required.")
            .IsEnumName(typeof(RelationshipToInsured), caseSensitive: false)
            .WithMessage($"Relationship to insured must be one of: {relationshipValues}.");
    }

    private static DateOnly GetLatestPossibleDate(TimeProvider timeProvider)
    {
        var latestDateTime = timeProvider.GetUtcNow().AddDays(1).Date;
        return DateOnly.FromDateTime(latestDateTime);
    }
}
