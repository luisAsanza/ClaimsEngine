using FluentValidation;
using ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;

namespace ClaimsEngine.Ingestion.Api.Features.Claims.Validators;

public sealed class SubmitClaimRequestValidator : AbstractValidator<SubmitClaimRequest>
{
    public SubmitClaimRequestValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.ClaimNumber)
            .NotEmpty().WithMessage("Claim number is required.")
            .MaximumLength(50);

        RuleFor(x => x.SubscriberId)
            .NotEmpty().WithMessage("SubscriberId is required.")
            .MaximumLength(50);

        RuleFor(x => x.PayerId)
            .NotEmpty().WithMessage("PayerId is required.")
            .MaximumLength(50);

        RuleFor(x => x.ProviderNpi)
            .NotEmpty().WithMessage("Provider NPI is required.")
            .MaximumLength(20);

        RuleFor(x => x.Patient)
            .NotNull().WithMessage("Patient is required.")
            .SetValidator(new PatientPayloadValidator(timeProvider));

        RuleFor(x => x.Insured)
            .NotNull().WithMessage("Insured is required.")
            .SetValidator(new InsuredPayloadValidator(timeProvider));

        RuleFor(x => x.LineItems)
            .NotEmpty().WithMessage("At least one line item is required.")
            .ForEach(li => li.SetValidator(new LineItemPayloadValidator()));
    }
}
