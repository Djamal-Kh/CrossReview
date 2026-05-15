using CrossReview.Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewPeriodConfiguration : IEntityTypeConfiguration<ReviewPeriod>
{
    public void Configure(EntityTypeBuilder<ReviewPeriod> builder)
    {
        builder.ToTable("review_periods");
        
        builder.HasKey(rp => rp.Id);

        builder.Property(rp => rp.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(rp => rp.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();
        
        builder.Property(rp => rp.StartDate)
            .HasColumnName("start_date")
            .IsRequired();
        
        builder.Property(rp => rp.EndDate)
            .HasColumnName("end_date")
            .IsRequired();
        
        builder.Property(rp => rp.Status)
            .HasConversion<int>()
            .HasColumnName("status")
            .IsRequired();

        builder.HasOne<ProjectEntity>()
            .WithMany(x => x.ReviewPeriods)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}