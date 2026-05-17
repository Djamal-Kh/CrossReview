using CrossReview.Application.Template;
using CrossReview.Domain.Template;

namespace CrossReview.Infrastructure.Postgres.Seeds;

public static class TemplateSeed
{
    public static async Task SeedAsync(
        ITemplateRepository repository,
        Guid projectId,
        CancellationToken ct)
    {
        var existing = await repository.GetAllAsync(ct);
        
        if (existing.Any(t => t.Title == "Default 360 Review"))
            return;

        var templates = new List<TemplateEntity>
        {
            CreateDefault360(projectId),
            CreateMinimal(projectId)
        };

        foreach (var template in templates)
        {
            await repository.AddAsync(template, ct);
        }

        await repository.SaveAsync(ct);
    }

    private static TemplateEntity CreateDefault360(Guid projectId)
    {
        var template = TemplateEntity.Create(projectId, "Default 360 Review");

        template.AddQuestion(template.Id, "Communication", 0.15);
        template.AddQuestion(template.Id, "TeamworkTooShort", 0.15);
        template.AddQuestion(template.Id, "Responsibility", 0.10);
        template.AddQuestion(template.Id, "Technical Skills", 0.20);
        template.AddQuestion(template.Id, "Problem Solving", 0.15);
        template.AddQuestion(template.Id, "Initiative", 0.15);
        template.AddQuestion(template.Id, "Reliability", 0.10);

        return template;
    }

    private static TemplateEntity CreateMinimal(Guid projectId)
    {
        var template = TemplateEntity.Create(projectId, "Minimal Review (3 questions)");

        template.AddQuestion(template.Id, "Overall performance", 0.4);
        template.AddQuestion(template.Id, "Team contribution", 0.3);
        template.AddQuestion(template.Id, "Goal achievement", 0.3);

        return template;
    }
}