using CrossReview.Domain.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewTemplateConfiguration : IEntityTypeConfiguration<ReviewTemplate>
{
    public void Configure(EntityTypeBuilder<ReviewTemplate> builder)
    {
        builder.ToTable("review_templates");
        
        builder
            .HasKey(rt => rt.Id)
            .HasName("pk_review_template");

        builder
            .Property(rt => rt.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("review_id");

        builder
            .Property(rt => rt.Title)
            .HasColumnName("title")
            .IsRequired();
        
        builder
            .HasMany(rt => rt.Questions)
            .WithOne()
            .HasForeignKey(q => q.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(rt => rt.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
    }
}