using CrossReview.Domain.Project;
using CrossReview.Domain.Review;
using Crossreview.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class EvaluationResultConfiguration : IEntityTypeConfiguration<EvaluationResultEntity>
{
    public void Configure(EntityTypeBuilder<EvaluationResultEntity> builder)
    {
        builder.ToTable("evaluation_results");
        
        builder
            .HasKey(er => er.Id)
            .HasName("pk_evaluation_result");

        builder
            .Property(er => er.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("evaluation_result_id")
            .IsRequired();
        
        builder
            .Property(er => er.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder
            .Property(er => er.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();
        
        builder
            .Property(er => er.PeriodId)
            .HasColumnName("period_id")
            .IsRequired();
        
        builder
            .Property(er => er.FinalScore)
            .HasColumnName("final_score")
            .IsRequired();
        
        builder
            .Property(er => er.CalculatedAt)
            .HasColumnName("calculated_at")
            .IsRequired();
        
        builder
            .HasOne<ReviewPeriod>()
            .WithMany()
            .HasForeignKey(er => er.PeriodId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_evaluation_result_period_id");
        
        builder
            .HasOne<ProjectEntity>()
            .WithMany()
            .HasForeignKey(er => er.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(er => er.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}