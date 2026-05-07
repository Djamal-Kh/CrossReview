using CrossReview.Application.User;
using CrossReview.Domain.User;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Common.ResultPattern;

namespace CrossReview.Infrastructure.Postgres.Repositories;

public class UserRepository(CrossReviewDbContext context) : IUserRepository
{
    public async Task<Guid> AddAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        
        return user;
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .AsNoTracking().
            FirstOrDefaultAsync(x => x.Email == email,
                cancellationToken);
        
        return user;
    }

    public async Task<List<UserEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return users;
    }

    public async Task<Guid> DeleteAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }

    public async Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user is null)
            return false;

        return true;
    }
}