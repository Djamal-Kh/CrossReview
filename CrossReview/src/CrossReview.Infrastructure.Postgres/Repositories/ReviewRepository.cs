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
            .Include(r => r.Answers)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return review;
    }

    public async Task<List<ReviewEntity?>> GetByReviewee(Guid userId, Guid projectId, Guid periodId,
        CancellationToken cancellationToken = default)
    {
        var reviews = await context.Reviews
            .Include(r => r.Answers)
            .Where(r => r.RevieweeId == userId 
                                                      && r.ProjectId == projectId 
                                                      && r.PeriodId == periodId)
            .ToListAsync(cancellationToken);

        return reviews;
    }

    public async Task<ReviewEntity?> GetByProject(Guid projectId, Guid revieweeId, Guid reviewerId, Guid periodId,
        CancellationToken cancellationToken = default)
    {
        var review = await context.Reviews
            .Include(r => r.Answers)
            .FirstOrDefaultAsync(r => r.ProjectId == projectId 
                && r.RevieweeId == revieweeId
                && r.ReviewerId == reviewerId
                && r.PeriodId == periodId, cancellationToken);

        return review;
    }

    public async Task<List<ReviewEntity>> GetAllAsync(Guid userId, Guid projectId, Guid periodId, CancellationToken cancellationToken = default)
    {
        var reviews = await context.Reviews
            .Include(r => r.Answers)
            .Where(r => r.RevieweeId == userId
                && r.ProjectId == projectId
                && r.PeriodId == periodId)
            .ToListAsync(cancellationToken);

        return reviews;
    }

    public async Task<List<ReviewEntity>> GetAllAsyncForSeed(Guid projectId, CancellationToken cancellationToken = default)
    {
        var result = await context.Reviews
            .Where(r => r.ProjectId == projectId)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<Guid?> DeleteAsync(ReviewEntity review, CancellationToken cancellationToken = default)
    {
        context.Reviews.Remove(review);
        
        await context.SaveChangesAsync(cancellationToken);
        return review.Id;
    }

    public async Task<bool> ExistsReviewAsync(Guid reviewerId, Guid revieweeId, Guid periodId, CancellationToken cancellationToken = default)
    {
        var review = await context.Reviews
            .FirstOrDefaultAsync(r => r.ReviewerId == reviewerId
                                      && r.RevieweeId == revieweeId
                                      && r.PeriodId == periodId);

        if (review is null)
            return false;

        return true;
    }
}