using CrossReview.Application.Project.UseCases.CloseReviewPeriod;
using CrossReview.Application.Review.UseCases.AddAnswerToReview;
using CrossReview.Application.Review.UseCases.CloseReview;
using CrossReview.Application.Review.UseCases.CreateReview;
using CrossReview.Application.Review.UseCases.GetReviewByParameters;
using CrossReview.Application.Review.UseCases.GetReviewsForProjectAndPeriod;
using CrossReview.Application.Review.UseCases.GetReviewsForUser;
using CrossReview.Application.Review.UseCases.RemoveAnswerFromReview;
using CrossReview.Application.Review.UseCases.SubmitReview;
using CrossReview.Application.Review.UseCases.UpdateAnswerInReview;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Review.Controllers;

[Route("api/review/")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly AddAnswerUseCase _addAnswerUseCase;
    private readonly CloseReviewUseCase _closeReviewUseCase;
    private readonly CreateReviewUseCase _createReviewUseCase;
    private readonly GetReviewByParametersUseCase _getReviewByParametersUseCase;
    private readonly GetProjectReviewsUseCase _getProjectReviewsUseCase;
    private readonly GetReviewsForUserUseCase _getReviewsForUserUseCase;
    private readonly RemoveAnswerUseCase _removeAnswerUseCase;
    private readonly SubmitReviewUseCase _submitReviewUseCase;
    private readonly UpdateAnswerUseCase _updateAnswerUseCase;
    
    public ReviewController(
        AddAnswerUseCase addAnswerUseCase,
        CloseReviewUseCase closeReviewUseCase,
        CreateReviewUseCase createReviewUseCase,
        GetReviewByParametersUseCase getReviewByParametersUseCase, 
        GetProjectReviewsUseCase getProjectReviewsUseCase, 
        GetReviewsForUserUseCase getReviewsForUserUseCase, 
        RemoveAnswerUseCase removeAnswerUseCase,
        SubmitReviewUseCase submitReviewUseCase, 
        UpdateAnswerUseCase updateAnswerUseCase)
    {
        _addAnswerUseCase = addAnswerUseCase;
        _closeReviewUseCase = closeReviewUseCase;
        _createReviewUseCase = createReviewUseCase;
        _getReviewByParametersUseCase = getReviewByParametersUseCase;
        _getProjectReviewsUseCase = getProjectReviewsUseCase;
        _getReviewsForUserUseCase = getReviewsForUserUseCase;
        _removeAnswerUseCase = removeAnswerUseCase;
        _submitReviewUseCase = submitReviewUseCase;
        _updateAnswerUseCase = updateAnswerUseCase;
    }
    
    [HttpPost]
    [Route("create")]
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
    public async Task<IActionResult> AddAnswer(
        Guid reviewId, 
        Guid questionId,
        int score,
        string comment,
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

    [HttpGet]
    [Route("by-parameters")]
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

    [HttpGet]
    [Route("by-reviewers")]
    public async Task<IActionResult> GetForUser(
        Guid userId,
        Guid projectId,
        Guid periodId,
        CancellationToken cancellationToken)
    {
        var request = new GetReviewsForUserRequest(userId, projectId, periodId);
        
        var result = await _getReviewsForUserUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPatch]
    [Route("close")]
    public async Task<IActionResult> Close(
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var request = new CloseReviewRequest(reviewId);

        var result = await _closeReviewUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("answer/remove")]
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
