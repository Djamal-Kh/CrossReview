using CrossReview.Domain.Template;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template;

public interface ITemplateRepository
{
    public Task<Guid> AddAsync(TemplateEntity template, CancellationToken cancellationToken = default);
    public Task SaveAsync(CancellationToken cancellationToken = default);
    public Task<TemplateEntity?> GetByIdAsync(Guid templateId, CancellationToken cancellationToken = default);
    public Task<Guid?> DeleteAsync(TemplateEntity template, CancellationToken cancellationToken = default);
    public Task<bool> ExistByTitleAsync(string title, CancellationToken cancellationToken = default);
}