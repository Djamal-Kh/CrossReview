using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CrossReview.Application.Project.UseCases.CloseReviewPeriod;
using CrossReview.Application.Review.UseCases.AddAnswerToReview;
using CrossReview.Application.Review.UseCases.CreateReview;
using CrossReview.Application.Review.UseCases.GenerateReviewsForPeriod;
using CrossReview.Application.Review.UseCases.GetAllEvaluatuinResults;
using CrossReview.Application.Review.UseCases.GetReviewByParameters;
using CrossReview.Application.Review.UseCases.GetReviewsForProjectAndPeriod;
using CrossReview.Application.Review.UseCases.GetReviewsForUser;
using CrossReview.Application.Review.UseCases.RemoveAnswerFromReview;
using CrossReview.Application.Review.UseCases.SubmitReview;
using CrossReview.Application.Review.UseCases.UpdateAnswerInReview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Review.Controllers;

[Route("api/review/")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly AddAnswerUseCase _addAnswerUseCase;
    private readonly CreateReviewUseCase _createReviewUseCase;
    private readonly GetReviewByParametersUseCase _getReviewByParametersUseCase;
    private readonly GetProjectReviewsUseCase _getProjectReviewsUseCase;
    private readonly GetReviewsForUserUseCase _getReviewsForUserUseCase;
    private readonly RemoveAnswerUseCase _removeAnswerUseCase;
    private readonly SubmitReviewUseCase _submitReviewUseCase;
    private readonly UpdateAnswerUseCase _updateAnswerUseCase;
    private readonly GenerateReviewsForPeriodUseCase _generateReviewsForPeriodUseCase;
    
    public ReviewController(
        AddAnswerUseCase addAnswerUseCase,
        CreateReviewUseCase createReviewUseCase,
        GetReviewByParametersUseCase getReviewByParametersUseCase, 
        GetProjectReviewsUseCase getProjectReviewsUseCase, 
        GetReviewsForUserUseCase getReviewsForUserUseCase, 
        RemoveAnswerUseCase removeAnswerUseCase,
        SubmitReviewUseCase submitReviewUseCase, 
        UpdateAnswerUseCase updateAnswerUseCase, 
        GenerateReviewsForPeriodUseCase generateReviewsForPeriodUseCase)
    {
        _addAnswerUseCase = addAnswerUseCase;
        _createReviewUseCase = createReviewUseCase;
        _getReviewByParametersUseCase = getReviewByParametersUseCase;
        _getProjectReviewsUseCase = getProjectReviewsUseCase;
        _getReviewsForUserUseCase = getReviewsForUserUseCase;
        _removeAnswerUseCase = removeAnswerUseCase;
        _submitReviewUseCase = submitReviewUseCase;
        _updateAnswerUseCase = updateAnswerUseCase;
        _generateReviewsForPeriodUseCase = generateReviewsForPeriodUseCase;
    }
    
    [HttpPost]
    [Route("generate")]
    [Authorize]
    public async Task<IActionResult> Generate(
        Guid projectId,
        Guid periodId,
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
    
        if (userId is null)
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        
        var request = new GenerateReviewsForPeriodRequest(
            projectId, periodId, templateId, userId.Value, isAdmin);

        var result = await _generateReviewsForPeriodUseCase.Execute(request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { created = result.Value });
    }

    private Guid? GetCurrentUserId()
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
        return Guid.TryParse(sub, out var id) ? id : null;
    }
    
    [HttpPost]
    [Route("create")]
    [Authorize]
    public async Task<IActionResult> Create(
        Guid reviewerId,
        Guid revieweeId,
        Guid projectId,
        Guid templateId,
        Guid periodId,
        CancellationToken cancellationToken)
    {
        var request = new CreateReviewRequest(reviewerId, revieweeId, projectId, templateId, periodId);

        var result = await _createReviewUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    [Route("answer/add")]
    [Authorize]
    public async Task<IActionResult> AddAnswer(
        Guid reviewId, 
        Guid questionId,
        int score,
        string? comment,
        CancellationToken cancellationToken)
    {
        var request = new AddAnswerRequest(reviewId, questionId, score, comment);
        
        var result = await _addAnswerUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    // А так ли было запланировано ?
    [HttpGet]
    [Route("id")]
    [Authorize]
    public async Task<IActionResult> GetByIdOrParameters(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetReviewByParametersRequest(id);
        
        var result = await _getReviewByParametersUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    // вызывается при переходе на страницу ревью
    [HttpGet]
    [Route("by-parameters")]
    [Authorize]
    public async Task<IActionResult> GetForProjectAndPeriod(
        Guid projectId,
        Guid revieweeId,
        Guid reviewerId,
        Guid periodId,
        CancellationToken cancellationToken)
    {
        var request = new GetProjectReviewsRequest(projectId, revieweeId, reviewerId, periodId);
        
        var result = await _getProjectReviewsUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    // вызывается
    [HttpGet]
    [Route("by-reviewers")]
    [Authorize]
    public async Task<IActionResult> GetForUser(
        Guid userId,
        Guid? projectId,
        Guid? periodId,
        CancellationToken cancellationToken)
    {
        var request = new GetReviewsForUserRequest(userId, projectId, periodId);
        
        var result = await _getReviewsForUserUseCase.Execute(request, cancellationToken);
        
        return Ok(result);
    }

    [HttpPatch]
    [Route("answer/remove")]
    [Authorize]
    public async Task<IActionResult> RemoveAnswer(
        Guid reviewId,
        Guid questionId,
        CancellationToken cancellationToken)
    {
        var request = new RemoveAnswerRequest(reviewId, questionId);

        var result = await _removeAnswerUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("submit")]
    [Authorize]
    public async Task<IActionResult> Submit(
        Guid reviewId,
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var request = new SubmitReviewRequest(reviewId, templateId);

        var result = await _submitReviewUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("answer/update")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAnswer(
        Guid reviewId,
        Guid questionId,
        int score,
        string comment,
        CancellationToken cancellationToken)
    {
        var request = new UpdateAnswerRequest(reviewId, questionId, score, comment);
        
        var result = await  _updateAnswerUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}
