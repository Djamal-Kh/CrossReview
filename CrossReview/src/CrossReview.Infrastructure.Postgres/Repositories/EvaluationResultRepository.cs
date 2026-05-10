using CrossReview.Application.Review;
using CrossReview.Application.Review.UseCases;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Common.ResultPattern;

namespace CrossReview.Infrastructure.Postgres.Repositories;

public class EvaluationResultRepository(CrossReviewDbContext context) : IEvaluationResultRepository
{
    public async Task<Guid> AddAsync(EvaluationResultEntity result, CancellationToken cancellationToken = default)
    {
        await context.EvaluationResults.AddAsync(result, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return result.Id;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<EvaluationResultEntity?> GetByParametersAsync(Guid userId, Guid projectId, Guid periodId, CancellationToken cancellationToken = default)
    {
        var result = await context.EvaluationResults
            .AsNoTracking()
            .FirstOrDefaultAsync(er => er.UserId == userId 
                                       && er.ProjectId == projectId 
                                       && er.PeriodId == periodId, cancellationToken);
        
        return result;
    }

    public async Task<EvaluationResultEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await context.EvaluationResults
            .AsNoTracking()
            .FirstOrDefaultAsync(er => er.Id == id, cancellationToken);

        return result;
    }

    public async Task<List<EvaluationResultEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var results = await context.EvaluationResults
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<Guid?> DeleteAsync(EvaluationResultEntity evaluationResult, CancellationToken cancellationToken = default)
    {
        context.EvaluationResults.Remove(evaluationResult);
        await context.SaveChangesAsync(cancellationToken);
        
        return evaluationResult.Id;
    }
}