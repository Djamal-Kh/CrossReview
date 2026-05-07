using CrossReview.Domain.Project;
using CrossReview.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("project_members");
        
        // Составной первичный ключ
        builder.HasKey(pm => new { pm.ProjectId, pm.UserId });
        
        builder.Property(pm => pm.ProjectId)
            .HasColumnName("project_id");
        
        builder.Property(pm => pm.UserId)
            .HasColumnName("user_id");
        
        builder.Property(pm => pm.Role)
            .HasConversion<int>()
            .HasColumnName("role")
            .IsRequired();
        
        builder.Property(pm => pm.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
        
        builder.Property(pm => pm.JoinedAt)
            .HasColumnName("joined_at")
            .IsRequired();
        
        builder.Property(pm => pm.LeftAt)
            .HasColumnName("left_at");
        
        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<ProjectEntity>()
            .WithMany(p => p.Members)
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}