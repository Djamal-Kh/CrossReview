using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project;

public interface IProjectRepository
{
    Task<Result<Guid, Error>> AddAsync(ProjectEntity project, CancellationToken cancellationToken = default);
    Task SaveAsync(ProjectEntity project, CancellationToken cancellationToken = default);
    Task<ProjectEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default);
}