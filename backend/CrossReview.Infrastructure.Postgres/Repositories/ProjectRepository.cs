using CrossReview.Application.Project;
using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Common.ResultPattern;

namespace CrossReview.Infrastructure.Postgres.Repositories;

public class ProjectRepository(CrossReviewDbContext context) : IProjectRepository
{
    public async Task<Guid> AddAsync(ProjectEntity project, CancellationToken cancellationToken = default)
    {
        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return project.Id;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        context.ChangeTracker.DetectChanges();
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ProjectEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await context.Projects
            .Include(p => p.Members)
            .Include(p => p.ReviewPeriods)
            .FirstOrDefaultAsync(p => p.Id == id, 
                cancellationToken);
        
        return project;
    }

    public async Task<List<ProjectEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projects = await context.Projects
            .Include(p => p.Members)
            .Include(p => p.ReviewPeriods)
            .ToListAsync(cancellationToken);

        return projects;
    }

    public async Task<List<ProjectEntity>> GetAllAsyncById(Guid userId, CancellationToken cancellationToken = default)
    {
        var projects = await context.Projects
            .Where(p => p.Members.Any(m => m.UserId == userId))
            .Include(p => p.Members)
            .Include(p => p.ReviewPeriods)
            .ToListAsync(cancellationToken);

        return projects;
    }

    public async Task<Guid?> DeleteAsync(ProjectEntity project, CancellationToken cancellationToken = default)
    {
        context.Projects.Remove(project);
        await context.SaveChangesAsync(cancellationToken);
        return project.Id;
    }

    public async Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Title == title, 
                cancellationToken);

        if (project is null)
            return false;

        return true;
    }
}