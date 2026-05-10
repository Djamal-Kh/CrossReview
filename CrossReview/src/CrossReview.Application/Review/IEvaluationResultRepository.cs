using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review;

public interface IEvaluationResultRepository
{
    public Task<Guid> AddAsync(EvaluationResultEntity resultEntity, CancellationToken cancellationToken = default);
    public Task SaveAsync(CancellationToken cancellationToken = default);
    public Task<EvaluationResultEntity?> GetByParametersAsync(Guid userId, Guid projectId, Guid periodId, CancellationToken cancellationToken = default);
    public Task<EvaluationResultEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<List<EvaluationResultEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Guid?> DeleteAsync(EvaluationResultEntity evaluationResult, CancellationToken cancellationToken = default);
}