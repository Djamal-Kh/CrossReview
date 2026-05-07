using CrossReview.Application.Template;
using CrossReview.Domain.Template;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Common.ResultPattern;

namespace CrossReview.Infrastructure.Postgres.Repositories;

public class TemplateRepository(CrossReviewDbContext context) : ITemplateRepository
{
    public async Task<Guid> AddAsync(TemplateEntity template, CancellationToken cancellationToken = default)
    {
        await context.ReviewTemplates.AddAsync(template, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return template.Id;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TemplateEntity?> GetByIdAsync(Guid templateId, CancellationToken cancellationToken = default)
    {
        var template = await context.ReviewTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken);
        
        return template;
    }

    public async Task<Guid?> DeleteAsync(TemplateEntity template, CancellationToken cancellationToken = default)
    {
        context.ReviewTemplates.Remove(template);
        await context.SaveChangesAsync(cancellationToken);
        
        return template.Id;
    }

    public async Task<bool> ExistByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        var template = await context.ReviewTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Title == title, cancellationToken);

        if (template is null)
            return false;

        return true;
    }
}