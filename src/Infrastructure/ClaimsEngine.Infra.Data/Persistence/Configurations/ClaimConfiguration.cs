using System;
using ClaimsEngine.Domain.Aggregates.ClaimAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;

namespace ClaimsEngine.Infra.Data.Persistence.Configurations
{
    internal sealed class ClaimConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.ToTable("Claims");
            builder.HasKey(c => c.Id);            

            builder.Property(c => c.RowVersion)
                .IsRowVersion();

            builder.Property(c => c.CorrelationId)
                .HasColumnName("CorrelationId")
                .IsRequired();

            builder.Property(c => c.ClaimNumber)
                .HasColumnName("ClaimNumber")
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(c => c.ClaimNumber)
                .IsUnique()
                .HasDatabaseName("UX_Claims_ClaimNumber");

            builder.Property(c => c.SubscriberId)
                .HasColumnName("SubscriberId")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.PayerId)
                .HasColumnName("PayerId")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.ProviderNpi)
                .HasColumnName("ProviderNpi")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Status)
                .HasColumnName("Status")
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(c => c.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .IsRequired();

            builder.ComplexProperty(x => x.Patient, p =>
            {
                p.Property(p => p.Name)
                .HasColumnName("PatientName")
                .IsRequired()
                .HasMaxLength(50);

                p.Property(p => p.DateOfBirth)
                .HasColumnName("PatientDateOfBirth");

                p.Property(p => p.RelationshipToInsured)
                .HasColumnName("PatientRelationshipToInsured")
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsUnicode(false);
            });

            builder.ComplexProperty(x => x.Insured, i =>
            {
                i.Property(i => i.Name)
                .HasColumnName("InsuredName")
                .IsRequired()
                .HasMaxLength(50);

                i.Property(i => i.DateOfBirth)
                .HasColumnName("InsuredDateOfBirth");
            });

            builder.HasMany(c => c.LineItems)
            .WithOne()
            .HasForeignKey("ClaimId")
            .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Claim.LineItems))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
