using CrossReview.Application.Review;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Common.ResultPattern;

namespace CrossReview.Infrastructure.Postgres.Repositories;

public class ReviewRepository(CrossReviewDbContext context) : IReviewRepository
{
    public async Task<Guid> AddAsync(ReviewEntity review, CancellationToken cancellationToken = default)
    {
        await context.Reviews.AddAsync(review, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return review.Id;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ReviewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var review = await context.Reviews
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return review;
    }

    public async Task<List<ReviewEntity?>> GetByReviewee(Guid userId, Guid projectId, Guid periodId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(); // СДЕЛАЙ
    }

    public async Task<ReviewEntity?> GetByProject(Guid projectId, Guid revieweeId, Guid reviewerId, Guid periodId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(); // СДЕЛАЙ
    }

    public async Task<List<ReviewEntity>> GetAllAsync(Guid userId, Guid projectId, Guid periodId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(); // СДЕЛАЙ
    }

    public async Task<Guid?> DeleteAsync(ReviewEntity review, CancellationToken cancellationToken = default)
    {
        context.Reviews.Remove(review);
        await context.SaveChangesAsync(cancellationToken);
        return review.Id;
    }

    public async Task<bool> ExistsReviewAsync(Guid reviewerId, Guid revieweeId, Guid periodId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(); // СДЕЛАЙ
    }
}