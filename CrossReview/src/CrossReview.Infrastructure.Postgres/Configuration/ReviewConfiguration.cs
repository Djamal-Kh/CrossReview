using CrossReview.Domain.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        
        builder
            .HasKey(r => r.Id)
            .HasName("pk_review");

        builder
            .Property(r => r.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("review_id")
            .IsRequired();

        builder
            .Property(r => r.ReviewerId)
            .HasColumnName("reviewer_id")
            .IsRequired();

        builder
            .Property(r => r.RevieweeId)
            .HasColumnName("reviewee_id")
            .IsRequired();

        builder
            .Property(r => r.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();

        builder
            .Property(r => r.TemplateId)
            .HasColumnName("template_id")
            .IsRequired();
        
        builder
            .HasMany(r => r.Answers)
            .WithOne()
            .HasForeignKey(ra => ra.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(r => r.PeriodId)
            .HasColumnName("period_id")
            .IsRequired();
    }
}