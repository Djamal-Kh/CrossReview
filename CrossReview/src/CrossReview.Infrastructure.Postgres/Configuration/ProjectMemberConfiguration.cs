using CrossReview.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("project_member");

        builder
            .Property(pm => pm.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder
            .Property(pm => pm.Role)
            .HasConversion<int>()
            .HasColumnName("role")
            .IsRequired();

        builder
            .Property(pm => pm.JoinedAt)
            .HasColumnName("joined_at")
            .IsRequired();

        builder
            .Property(pm => pm.LeftAt)
            .HasColumnName("left_at");
    }
}