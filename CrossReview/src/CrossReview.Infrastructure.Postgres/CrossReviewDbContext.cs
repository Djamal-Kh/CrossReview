using CrossReview.Domain.Project;
using CrossReview.Domain.Review;
using CrossReview.Domain.Template;
using CrossReview.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace CrossReview.Infrastructure.Postgres;

public class CrossReviewDbContext(DbContextOptions<CrossReviewDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CrossReviewDbContext).Assembly);
    }
    
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<ReviewPeriod> ReviewPeriods { get; set; }
    
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<TemplateEntity> ReviewTemplates { get; set; }
    public DbSet<ReviewAnswer> ReviewAnswers { get; set; } 
    public DbSet<ReviewQuestion> ReviewQuestions { get; set; } 
    public DbSet<EvaluationResultEntity> EvaluationResults { get; set; }
}