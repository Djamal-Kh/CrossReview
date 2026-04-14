using CrossReview.Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("projects");
        
        builder
            .HasKey(p => p.Id)
            .HasName("pk_projects_id");

        builder
            .Property(p => p.Id)
            .HasColumnName("project_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(p => p.Title)
            .HasColumnName("title")
            .IsRequired();

        builder
            .Property(p => p.Status)
            .HasColumnName("is_active")
            .IsRequired();
        
        builder
            .Property(p => p.Description)
            .HasColumnName("description");
        
        builder
            .HasMany(p => p.Members)
            .WithOne()
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(p => p.Periods)
            .WithOne()
            .HasForeignKey(rp => rp.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}