using CrossReview.Domain.Project;
using CrossReview.Domain.Review;
using CrossReview.Domain.Template;
using Crossreview.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
{
    public void Configure(EntityTypeBuilder<ReviewEntity> builder)
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
            .Property(r => r.PeriodId)
            .HasColumnName("period_id")
            .IsRequired();
        
        builder.Property(r => r.Status)
            .HasConversion<int>()
            .HasColumnName("status")
            .IsRequired();
        
        builder
            .HasOne<ReviewPeriod>()
            .WithMany()
            .HasForeignKey(r => r.PeriodId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_review_period_id");
        
        builder
            .HasOne<ProjectEntity>()
            .WithMany()
            .HasForeignKey(r => r.ProjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_project_id");
        
        builder
            .HasOne<TemplateEntity>()
            .WithMany()
            .HasForeignKey(r => r.TemplateId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_template_id");
        
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(r => r.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}