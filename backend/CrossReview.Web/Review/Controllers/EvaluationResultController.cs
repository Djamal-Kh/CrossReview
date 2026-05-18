using CrossReview.Application.Review.UseCases.CalculateEvaluationResult;
using CrossReview.Application.Review.UseCases.GetEvaluationResult;
using CrossReview.Application.Review.UseCases.RecalculateEvaluationResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Review.Controllers;

[Route("api/review/evaluation-result/")]
[ApiController]
public class EvaluationResultController : ControllerBase
{
    private readonly CalculateEvaluationResultUseCase _calculateEvaluationResultUseCase;
    private readonly GetEvaluationResultUseCase _getEvaluationResultUseCase;
    private readonly RecalculateEvaluationResultUseCase _recalculateEvaluationResultUseCase;
    
    public EvaluationResultController(
        CalculateEvaluationResultUseCase calculateEvaluationResultUseCase, 
        GetEvaluationResultUseCase getEvaluationResultUseCase,
        RecalculateEvaluationResultUseCase recalculateEvaluationResultUseCase)
    {
        _calculateEvaluationResultUseCase = calculateEvaluationResultUseCase;
        _getEvaluationResultUseCase = getEvaluationResultUseCase;
        _recalculateEvaluationResultUseCase = recalculateEvaluationResultUseCase;
    }

    [HttpPost]
    [Route("calculate")]
    [Authorize]
    public async Task<IActionResult> Calculate(
        Guid userId,
        Guid projectId,
        Guid periodId,
        CancellationToken cancellationToken)
    {
        var request = new CalculateEvaluationResultRequest(userId, projectId, periodId);
        
        var result = await _calculateEvaluationResultUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpGet]
    [Route("by-parameters")]
    [Authorize]
    public async Task<IActionResult> Get(
        Guid userId,
        Guid projectId,
        Guid periodId,
        CancellationToken cancellationToken)
    {
        var request = new GetEvaluationResultRequest(userId, projectId, periodId);

        var result = await _getEvaluationResultUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("recalculate")]
    [Authorize]
    public async Task<IActionResult> Recalculate(
        Guid evaluationResultId,
        CancellationToken cancellationToken)
    {
        var request = new RecalculateEvaluationResultRequest(evaluationResultId);

        var result = await _recalculateEvaluationResultUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}