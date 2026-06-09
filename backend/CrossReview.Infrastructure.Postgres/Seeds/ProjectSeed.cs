using CrossReview.Application.Project;
using CrossReview.Domain.Project;
using Crossreview.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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
        IServiceProvider sp,
        CancellationToken ct)
    {
        // Получаем реальных пользователей из БД
        var userManager = sp.GetRequiredService<UserManager<AppUser>>();
        
        var adminUser = await userManager.FindByEmailAsync("admin@system.local");
        if (adminUser == null)
            throw new Exception("Admin user not found!");
            
        var regularUser = await userManager.FindByEmailAsync("user@system.local");
        if (regularUser == null)
        {
            // Создаем обычного пользователя
            regularUser = new AppUser
            {
                UserName = "user@system.local",
                Email = "user@system.local",
                FirstName = "Regular",
                LastName = "User"
            };
            await userManager.CreateAsync(regularUser, "User123!");
            await userManager.AddToRoleAsync(regularUser, "User");
        }
        
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

        // Используем РЕАЛЬНЫЕ ID из БД
        var project = CreateDefaultProject(adminUser.Id, regularUser.Id, out var createPeriodId);

        await repository.AddAsync(project, ct);
        await repository.SaveAsync(ct);

        return new ResultSeed(
            project,
            adminUser.Id,
            regularUser.Id,
            createPeriodId);
    }

    private static ProjectEntity CreateDefaultProject(
        Guid adminId, 
        Guid userId, 
        out Guid periodId)
    {
        var project = ProjectEntity.Create(
            "CrossReview Demo Project",
            "Default project for demonstration purposes");

        project.AssignEmployeeToProject(adminId, EnumProjectRole.TeamLead);
        project.AssignEmployeeToProject(userId, EnumProjectRole.Developer);

        periodId = project.CreateReviewPeriod(
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(30));

        project.ActivateReviewPeriod(periodId);

        return project;
    }
}