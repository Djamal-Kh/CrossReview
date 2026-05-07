using CrossReview.Domain;
using CrossReview.Domain.User;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User;

public interface IUserRepository
{
    public Task<Guid> AddAsync(UserEntity user, CancellationToken cancellationToken = default);
    public Task SaveAsync(CancellationToken cancellationToken = default);
    public Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<List<UserEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Guid> DeleteAsync(UserEntity user, CancellationToken cancellationToken = default);
    public Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken = default);
}