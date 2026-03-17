namespace ClaimsEngine.Domain.Aggregates.ClaimAggregate;

public sealed record Patient
{
    public string Name { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public RelationshipToInsured RelationshipToInsured { get; init; }

    public Patient(string name, DateOnly? dateOfBirth, RelationshipToInsured relationshipToInsured)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        DateOfBirth = dateOfBirth;
        RelationshipToInsured = relationshipToInsured;
    }

    #region EF Core materialization constructor
    
    public Patient() {
        // EF Core requires a parameterless constructor for materialization.
        // Compiler enforces that all properties are initialized, to avoid the warning 
        // about non-nullable properties not being initialized.
        Name = null!;
        RelationshipToInsured = default!;
    }

    #endregion
}
