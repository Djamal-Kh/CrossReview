using CrossReview.Domain.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewQuestionConfiguration : IEntityTypeConfiguration<ReviewQuestion>
{
    public void Configure(EntityTypeBuilder<ReviewQuestion> builder)
    {
        builder.ToTable("review_questions");
        
        builder
            .HasKey(rq => rq.Id)
            .HasName("pk_question");

        builder
            .Property(rq => rq.Id)
            .HasColumnName("question_id")
            .IsRequired();
        
        builder
            .Property(rq => rq.Text)
            .HasColumnName("text")
            .IsRequired();

        builder
            .Property(rq => rq.Weight)
            .HasColumnName("weight")
            .IsRequired();
    }
}