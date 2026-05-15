using CrossReview.Application.Project;
using CrossReview.Domain.Project;

namespace CrossReview.Infrastructure.Postgres.Seeds;

public class ProjectSeed
{
    public record ResultSeed(
        ProjectEntity Project,
        Guid AdminId,
        Guid UserId,
        Guid ActivePeriodId);

    public static async Task<ResultSeed> SeedAsync(
        IProjectRepository repository,
        CancellationToken ct)
    {
        var existingProject = (await repository.GetAllAsync(ct))
            .FirstOrDefault(p => p.Title == "CrossReview Demo Project");

        if (existingProject is not null)
        {
            var admin = existingProject.Members
                .First(m => m.Role == EnumProjectRole.TeamLead)
                .UserId;

            var user = existingProject.Members
                .First(m => m.Role == EnumProjectRole.Developer)
                .UserId;

            var periodId = existingProject.ReviewPeriods.First().Id;

            return new ResultSeed(existingProject, admin, user, periodId);
        }

        var project = CreateDefaultProject(out var createPeriodId);

        await repository.AddAsync(project, ct);
        await repository.SaveAsync(ct);

        return new ResultSeed(
            project,
            SeedUsers.AdminId,
            SeedUsers.UserId,
            createPeriodId);
    }

    private static ProjectEntity CreateDefaultProject(out Guid periodId)
    {
        var project = ProjectEntity.Create(
            "CrossReview Demo Project",
            "Default project for demonstration purposes");

        project.AssignEmployeeToProject(SeedUsers.AdminId, EnumProjectRole.TeamLead);
        project.AssignEmployeeToProject(SeedUsers.UserId, EnumProjectRole.Developer);

        periodId = project.CreateReviewPeriod(
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(30));

        project.ActivateReviewPeriod(periodId);

        return project;
    }
}