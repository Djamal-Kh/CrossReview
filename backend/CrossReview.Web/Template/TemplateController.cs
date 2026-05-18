using CrossReview.Application.Template.UseCases.ActivateTemplate;
using CrossReview.Application.Template.UseCases.AddQuestionToTemplate;
using CrossReview.Application.Template.UseCases.CreateTemplate;
using CrossReview.Application.Template.UseCases.DeactivateTemplate;
using CrossReview.Application.Template.UseCases.DeleteTemplate;
using CrossReview.Application.Template.UseCases.GetAllTemplates;
using CrossReview.Application.Template.UseCases.GetTemplateById;
using CrossReview.Application.Template.UseCases.RemoveQuestionFromTemplate;
using CrossReview.Application.Template.UseCases.ReorderQuestions;
using CrossReview.Application.Template.UseCases.UpdateQuestionInTemplate;
using CrossReview.Application.Template.UseCases.UpdateTemplateTitle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Template;

[Route("api/template/")]
[ApiController]
public class TemplateController : ControllerBase
{
    private readonly ActivateTemplateUseCase _activateTemplateUseCase;
    private readonly AddQuestionUseCase _addQuestionUseCase;
    private readonly CreateTemplateUseCase _createTemplateUseCase;
    private readonly DeactivateTemplateUseCase _deactivateTemplateUseCase;
    private readonly DeleteTemplateUseCase _deleteTemplateUseCase;
    private readonly RemoveQuestionUseCase _removeQuestionUseCase;
    private readonly ReorderQuestionsUseCase _reorderQuestionsUseCase;
    private readonly UpdateQuestionUseCase _updateQuestionUseCase;
    private readonly UpdateTemplateTitleUseCase _updateTemplateTitleUseCase;
    private readonly GetTemplateByIdUseCase _getTemplateByIdUseCase;
    private readonly GetAllTemplateUseCase _getAllTemplatesUseCase;
    
    public TemplateController(
        ActivateTemplateUseCase activateTemplateUseCase,
        AddQuestionUseCase addQuestionUseCase,
        CreateTemplateUseCase createTemplateUseCase, 
        DeactivateTemplateUseCase deactivateTemplateUseCase,
        DeleteTemplateUseCase deleteTemplateUseCase,
        RemoveQuestionUseCase removeQuestionUseCase,
        ReorderQuestionsUseCase reorderQuestionsUseCase,
        UpdateQuestionUseCase updateQuestionUseCase, 
        UpdateTemplateTitleUseCase updateTemplateTitleUseCase,
        GetTemplateByIdUseCase getTemplateByIdUseCase, 
        GetAllTemplateUseCase getAllTemplatesUseCase)
    {
        _activateTemplateUseCase = activateTemplateUseCase;
        _addQuestionUseCase = addQuestionUseCase;
        _createTemplateUseCase = createTemplateUseCase;
        _deactivateTemplateUseCase = deactivateTemplateUseCase;
        _deleteTemplateUseCase = deleteTemplateUseCase;
        _removeQuestionUseCase = removeQuestionUseCase;
        _reorderQuestionsUseCase = reorderQuestionsUseCase;
        _updateQuestionUseCase = updateQuestionUseCase;
        _updateTemplateTitleUseCase = updateTemplateTitleUseCase;
        _getTemplateByIdUseCase = getTemplateByIdUseCase;
        _getAllTemplatesUseCase = getAllTemplatesUseCase;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        Guid projectId,
        string title,
        CancellationToken cancellationToken)
    {
        var request = new CreateTemplateRequest(projectId, title);
        
        var result = await _createTemplateUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPatch]
    [Route("activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activate(
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var request = new ActivateTemplateRequest(templateId);

        var result = await _activateTemplateUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("question/add")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddQuestion(
        Guid templateId,
        string title,
        double weight,
        CancellationToken cancellationToken)
    {
        var request = new AddQuestionRequest(templateId, title, weight);

        var result = await _addQuestionUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("all")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllTemplatesUseCase.Execute(cancellationToken);
        
        if (result.IsFailure) 
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("{templateId:guid}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> GetById(
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var request = new GetTemplateByIdRequest(templateId);

        var result = await _getTemplateByIdUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPatch]
    [Route("deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var request = new DeactivateTemplateRequest(templateId);

        var result = await _deactivateTemplateUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("question/reorder")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> Reorder(
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var request = new ReorderQuestionsRequest(templateId);

        var result = await _reorderQuestionsUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("update-title")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTitle(
        Guid templateId,
        string title,
        CancellationToken cancellationToken)
    {
        var request = new UpdateTemplateTitleRequest(templateId, title);
        
        var result = await _updateTemplateTitleUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPut]
    [Route("question/update")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateQuestion(
        Guid templateId,
        Guid questionId,
        string title,
        double weight,
        CancellationToken cancellationToken)
    {
        var request = new UpdateQuestionRequest(templateId, questionId, title, weight);
        
        var result = await _updateQuestionUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpDelete]
    [Route("delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var request = new DeleteTemplateRequest(templateId);

        var result = await _deleteTemplateUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpDelete]
    [Route("question/remove")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveQuestion(
        Guid templateId,
        Guid questionId,
        CancellationToken cancellationToken)
    {
        var request = new RemoveQuestionRequest(templateId, questionId);

        var result = await _removeQuestionUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}