using CrossReview.Domain.Review;
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
    }
}