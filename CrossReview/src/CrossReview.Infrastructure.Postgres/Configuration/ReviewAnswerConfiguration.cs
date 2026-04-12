using CrossReview.Domain.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewAnswerConfiguration : IEntityTypeConfiguration<ReviewAnswer>
{
    public void Configure(EntityTypeBuilder<ReviewAnswer> builder)
    {
        builder.ToTable("review_answer");

        builder
            .Property(ra => ra.QuestionId)
            .HasColumnName("question_id")
            .IsRequired();

        builder
            .Property(ra => ra.Score)
            .HasColumnName("score")
            .IsRequired();

        builder
            .Property(ra => ra.Comment)
            .HasColumnName("comment");
    }
}