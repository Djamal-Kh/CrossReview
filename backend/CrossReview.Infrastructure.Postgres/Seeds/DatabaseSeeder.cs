using CrossReview.Application.Project;
using CrossReview.Application.Review;
using CrossReview.Application.Template;
using Microsoft.Extensions.DependencyInjection;

namespace CrossReview.Infrastructure.Postgres.Seeds;

public class DatabaseSeeder
{
    public async Task SeedAsync(IServiceProvider sp)
    {
        var projectRepo = sp.GetRequiredService<IProjectRepository>();
        var templateRepo = sp.GetRequiredService<ITemplateRepository>();
        var reviewRepo = sp.GetRequiredService<IReviewRepository>();

        var projectResult =
            await ProjectSeed.SeedAsync(projectRepo, CancellationToken.None);

        await TemplateSeed.SeedAsync(
            templateRepo,
            projectResult.Project.Id,
            CancellationToken.None);

        var template = (await templateRepo.GetAllAsync(CancellationToken.None))
            .First();

        await ReviewSeed.SeedAsync(
            reviewRepo,
            projectResult.Project.Id,
            template.Id,
            projectResult.AdminId,
            projectResult.UserId,
            projectResult.ActivePeriodId,
            CancellationToken.None);
    }
}