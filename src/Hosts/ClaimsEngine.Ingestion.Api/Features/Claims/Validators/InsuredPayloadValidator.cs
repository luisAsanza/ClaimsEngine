using System;
using FluentValidation;
using ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;

namespace ClaimsEngine.Ingestion.Api.Features.Claims.Validators;

public sealed class InsuredPayloadValidator : AbstractValidator<InsuredPayload>
{
    public InsuredPayloadValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Insured name is required.")
            .MaximumLength(200);

        RuleFor(x => x.DateOfBirth)
            .LessThan(GetLatestPossibleDate(timeProvider)).WithMessage("Date of birth must be in the past.");
    }

    private static DateOnly GetLatestPossibleDate(TimeProvider timeProvider)
    {
        var latestDateTime = timeProvider.GetUtcNow().AddDays(1).Date;
        return DateOnly.FromDateTime(latestDateTime);
    }
}
