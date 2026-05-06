using CrossReview.Domain.Template;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template;

public interface ITemplateRepository
{
    public Task<Result<Guid, Error>> AddAsync(TemplateEntity template, CancellationToken cancellationToken = default);
    public Task SaveAsync(TemplateEntity template, CancellationToken cancellationToken = default);
    public Task<TemplateEntity?> GetByIdAsync(Guid templateId, CancellationToken cancellationToken = default);
    public Task<Guid?> DeleteAsync(Guid templateId, CancellationToken cancellationToken = default);
    public Task<bool> ExistByTitleAsync(string title, CancellationToken cancellationToken = default);
}