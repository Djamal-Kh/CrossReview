using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project;

public interface IProjectRepository
{
    public Task<Guid> AddAsync(ProjectEntity project, CancellationToken cancellationToken = default);
    public Task SaveAsync(CancellationToken cancellationToken = default);
    public Task<ProjectEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<List<ProjectEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Guid?> DeleteAsync(ProjectEntity project, CancellationToken cancellationToken = default);
    public Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default);
}