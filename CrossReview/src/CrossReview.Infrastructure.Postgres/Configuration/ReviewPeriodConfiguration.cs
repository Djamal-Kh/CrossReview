using CrossReview.Domain.Review.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewPeriodConfiguration : IEntityTypeConfiguration<ReviewPeriod>
{
    public void Configure(EntityTypeBuilder<ReviewPeriod> builder)
    {
        builder.ToTable("review_periods");

        builder
            .HasKey(rp => rp.Id)
            .HasName("pk_period_id");
        
        builder
            .Property(rp => rp.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(rp => rp.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder
            .Property(rp => rp.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder
            .Property(rp => rp.Status)
            .HasConversion<int>()
            .HasColumnName("status")
            .IsRequired();
    }
}