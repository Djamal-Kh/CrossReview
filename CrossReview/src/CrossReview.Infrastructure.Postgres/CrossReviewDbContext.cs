using CrossReview.Domain.Project;
using CrossReview.Domain.Review;
using CrossReview.Domain.Template;
using Crossreview.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrossReview.Infrastructure.Postgres;

public class CrossReviewDbContext
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public CrossReviewDbContext(DbContextOptions<CrossReviewDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CrossReviewDbContext).Assembly);
    }

    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<ReviewPeriod> ReviewPeriods { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<TemplateEntity> ReviewTemplates { get; set; }
    public DbSet<ReviewAnswer> ReviewAnswers { get; set; } 
    public DbSet<ReviewQuestion> ReviewQuestions { get; set; } 
    public DbSet<EvaluationResultEntity> EvaluationResults { get; set; }
}
