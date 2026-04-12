using CrossReview.Domain.Project;
using CrossReview.Domain.Project.ValueObjects;
using CrossReview.Domain.Review;
using CrossReview.Domain.Review.ValueObjects;
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
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<ReviewPeriod> ReviewPeriods { get; set; }
    
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ReviewTemplate> ReviewTemplates { get; set; }
    public DbSet<ReviewAnswer> ReviewAnswers { get; set; } 
    public DbSet<ReviewQuestion> ReviewQuestions { get; set; } 
    public DbSet<EvaluationResult> EvaluationResults { get; set; }
}