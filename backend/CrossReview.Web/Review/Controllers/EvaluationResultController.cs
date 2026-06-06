using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CrossReview.Application.Review.UseCases.CalculateEvaluationResult;
using CrossReview.Application.Review.UseCases.GetAllEvaluatuinResults;
using CrossReview.Application.Review.UseCases.GetEvaluationResult;
using CrossReview.Application.Review.UseCases.GetEvaluationResultsByProjectId;
using CrossReview.Application.Review.UseCases.GetEvaluationResulyByUserId;
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
    private readonly GetAllEvaluationResultsUseCase _getAllEvaluationResultsUseCase;
    private readonly GetEvaluationResultByUserIdUseCase _getEvaluationResultByUserIdUseCase;
    private readonly GetEvaluationResultsByProjectIdUseCase _getEvaluationResultsByProjectIdUseCase;
    
    public EvaluationResultController(
        CalculateEvaluationResultUseCase calculateEvaluationResultUseCase, 
        GetEvaluationResultUseCase getEvaluationResultUseCase,
        RecalculateEvaluationResultUseCase recalculateEvaluationResultUseCase, 
        GetAllEvaluationResultsUseCase getAllEvaluationResultsUseCase, 
        GetEvaluationResultByUserIdUseCase getEvaluationResultByUserIdUseCase,
        GetEvaluationResultsByProjectIdUseCase getEvaluationResultsByProjectIdUseCase)
    {
        _calculateEvaluationResultUseCase = calculateEvaluationResultUseCase;
        _getEvaluationResultUseCase = getEvaluationResultUseCase;
        _recalculateEvaluationResultUseCase = recalculateEvaluationResultUseCase;
        _getAllEvaluationResultsUseCase = getAllEvaluationResultsUseCase;
        _getEvaluationResultByUserIdUseCase = getEvaluationResultByUserIdUseCase;
        _getEvaluationResultsByProjectIdUseCase = getEvaluationResultsByProjectIdUseCase;
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
    [Route("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var results = await _getAllEvaluationResultsUseCase.Execute(cancellationToken);

        return Ok(results);
    }

    [HttpGet]
    [Route("my")]
    [Authorize]
    public async Task<IActionResult> GetMy(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        
        if (userId is null) return Unauthorized();

        var request = new GetEvaluationResultByUserIdRequest(userId);
        
        var result = await _getEvaluationResultByUserIdUseCase.Execute(request, cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("by-project/{projectId}")]
    [Authorize]
    public async Task<IActionResult> GetByProject(
        Guid projectId, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();
        
        var isAdminRole = User.IsInRole("Admin");
        
        var request = new GetEvaluationResultsByProjectIdRequest(userId, projectId, isAdminRole);
        
        var result = await _getEvaluationResultsByProjectIdUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpGet]
    [Route("by-parameters")]
    [Authorize]
    public async Task<IActionResult> Get(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var request = new GetEvaluationResultRequest(userId);

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
    
    private Guid? GetCurrentUserId()
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(sub, out var id) ? id : null;
    }
}