using CrossReview.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossReview.Infrastructure.Postgres.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        
        builder
            .HasKey(u => u.Id)
            .HasName("pk_user");
        
        builder
            .Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("user_id")
            .IsRequired();

        builder
            .Property(u => u.FirstName)
            .HasMaxLength(50)
            .HasColumnName("first_name")
            .IsRequired();
        
        builder
            .Property(u => u.LastName)
            .HasMaxLength(50)
            .HasColumnName("last_name")
            .IsRequired();

        builder
            .Property(u => u.Email)
            .HasMaxLength(255)
            .HasColumnName("email")
            .IsRequired();
        
        builder
            .Property(u => u.PhoneNumber)
            .HasMaxLength(20)
            .HasColumnName("phone_number");
        
        builder
            .Property(u => u.Role)
            .HasConversion<int>()
            .HasColumnName("role")
            .IsRequired();

        builder
            .Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();
        
        builder.HasIndex(x => x.Email).IsUnique();
    }
}