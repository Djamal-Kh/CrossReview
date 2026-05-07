using CrossReview.Domain.Review;
using CrossReview.Domain.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewAnswerConfiguration : IEntityTypeConfiguration<ReviewAnswer>
{
    public void Configure(EntityTypeBuilder<ReviewAnswer> builder)
    {
        builder.ToTable("review_answers");
        
        builder.HasKey(ra => new { ra.ReviewId, ra.QuestionId });
        
        builder.Property(ra => ra.ReviewId)
            .HasColumnName("review_id");
        
        builder.Property(ra => ra.QuestionId)
            .HasColumnName("question_id");
        
        builder.Property(ra => ra.Score)
            .HasColumnName("score")
            .IsRequired();
        
        builder.Property(ra => ra.Comment)
            .HasColumnName("comment");
        
        builder.HasOne<ReviewEntity>()
            .WithMany(r => r.Answers)
            .HasForeignKey(ra => ra.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<ReviewQuestion>()
            .WithMany()
            .HasForeignKey(ra => ra.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}