using ClaimsEngine.Domain.Aggregates.ClaimAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsEngine.Infra.Data.Persistence.Configurations
{
    internal sealed class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
    {
        public void Configure(EntityTypeBuilder<LineItem> builder)
        {
            builder.ToTable("LineItems");
            builder.HasKey(li => li.Id);

            builder.Property(li => li.Description)
                .HasColumnName("Description")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(li => li.Amount)
                .HasColumnName("Amount")
                .IsRequired()
                .HasPrecision(18, 4);
        }
    }
}
