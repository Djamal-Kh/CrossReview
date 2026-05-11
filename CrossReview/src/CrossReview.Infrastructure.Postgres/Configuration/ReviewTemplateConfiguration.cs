using CrossReview.Domain.Project;
using CrossReview.Domain.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ReviewTemplateConfiguration : IEntityTypeConfiguration<TemplateEntity>
{
    public void Configure(EntityTypeBuilder<TemplateEntity> builder)
    {
        builder.ToTable("review_templates");
        
        builder
            .HasKey(rt => rt.Id)
            .HasName("pk_review_template");

        builder
            .Property(rt => rt.ProjectId)
            .HasColumnName("project_id");
        
        builder
            .Property(rt => rt.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("review_template_id");

        builder
            .Property(rt => rt.Title)
            .HasColumnName("title")
            .IsRequired();

        builder
            .Property(rt => rt.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder
            .HasMany(t => t.Questions)
            .WithOne()
            .HasForeignKey(q => q.TemplateId);
        
        builder.HasOne<ProjectEntity>()
            .WithMany()
            .HasForeignKey(x => x.ProjectId);
    }
}