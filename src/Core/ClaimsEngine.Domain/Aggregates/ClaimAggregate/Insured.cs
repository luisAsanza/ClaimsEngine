namespace ClaimsEngine.Domain.Aggregates.ClaimAggregate;

public sealed record Insured
{
    public string Name { get; init; }
    public DateOnly? DateOfBirth { get; init; }

    public Insured(string name, DateOnly? dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        DateOfBirth = dateOfBirth;
    }
}
