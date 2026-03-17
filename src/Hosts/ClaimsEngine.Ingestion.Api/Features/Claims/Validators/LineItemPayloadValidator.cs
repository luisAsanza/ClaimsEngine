using FluentValidation;
using ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;

namespace ClaimsEngine.Ingestion.Api.Features.Claims.Validators;

public sealed class LineItemPayloadValidator : AbstractValidator<LineItemPayload>
{
    public LineItemPayloadValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0.01M).WithMessage("Amount must be at least 0.01.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500);

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Quantity must be at least 1.");
    }
}
