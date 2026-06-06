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

        // Ответственность (3.0)
        template.AddQuestion(template.Id,
            "Соблюдает сроки выполнения задач", 0.75);

        template.AddQuestion(template.Id,
            "Выполняет взятые обязательства", 0.75);

        template.AddQuestion(template.Id,
            "Ответственно относится к качеству работы", 0.75);

        template.AddQuestion(template.Id,
            "Самостоятельно контролирует результаты работы", 0.75);

        // Командная работа и коммуникация (2.5)
        template.AddQuestion(template.Id,
            "Эффективно взаимодействует с коллегами", 0.625);

        template.AddQuestion(template.Id,
            "Четко формулирует мысли и задачи", 0.625);

        template.AddQuestion(template.Id,
            "Своевременно предоставляет обратную связь", 0.625);

        template.AddQuestion(template.Id,
            "Учитывает мнение других участников команды", 0.625);

        // Профессиональные навыки (2.0)
        template.AddQuestion(template.Id,
            "Демонстрирует достаточный уровень знаний", 0.5);

        template.AddQuestion(template.Id,
            "Применяет знания на практике", 0.5);

        template.AddQuestion(template.Id,
            "Быстро осваивает новые технологии", 0.5);

        template.AddQuestion(template.Id,
            "Применяет эффективные методы работы", 0.5);

        // Решение проблем (1.5)
        template.AddQuestion(template.Id,
            "Способен анализировать возникающие проблемы", 0.375);

        template.AddQuestion(template.Id,
            "Предлагает рациональные решения", 0.375);

        template.AddQuestion(template.Id,
            "Принимает обоснованные решения", 0.375);

        template.AddQuestion(template.Id,
            "Умеет работать в нестандартных ситуациях", 0.375);


        template.AddQuestion(template.Id,
            "Проявляет инициативу", 0.25);

        template.AddQuestion(template.Id,
            "Предлагает идеи по улучшению процессов", 0.25);

        template.AddQuestion(template.Id,
            "Самостоятельно ищет новые подходы", 0.25);

        template.AddQuestion(template.Id,
            "Готов брать дополнительную ответственность", 0.25);

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