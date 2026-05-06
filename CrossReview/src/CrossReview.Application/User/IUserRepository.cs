using CrossReview.Domain.Project;
using CrossReview.Domain.User;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User;

public interface IUserRepository
{
    public Task<Result<Guid, Error>> AddAsync(UserEntity project, CancellationToken cancellationToken = default);
    public Task SaveAsync(UserEntity project, CancellationToken cancellationToken = default);
    public Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<List<UserEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Guid> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken = default);
}