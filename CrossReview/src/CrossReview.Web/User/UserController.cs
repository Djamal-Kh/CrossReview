using CrossReview.Application.User.UseCases.CreateUser__Register_;
using CrossReview.Application.User.UseCases.DeleteUser;
using CrossReview.Application.User.UseCases.GetUserByEmail;
using CrossReview.Application.User.UseCases.GetUserById;
using CrossReview.Domain.User;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.User;

[Route("api/user/")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    private readonly DeleteUserUseCase _deleteUserUseCase;
    private readonly GetUserByEmailUseCase _getUserByEmailUseCase;
    private readonly GetUserByIdUseCase _getUserByIdUseCase;
    
    public UserController(
        CreateUserUseCase createUserUseCase,
        DeleteUserUseCase deleteUserUseCase, 
        GetUserByEmailUseCase getUserByEmailUseCase, 
        GetUserByIdUseCase getUserByIdUseCase)
    {
        _createUserUseCase = createUserUseCase;
        _deleteUserUseCase = deleteUserUseCase;
        _getUserByEmailUseCase = getUserByEmailUseCase;
        _getUserByIdUseCase = getUserByIdUseCase;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(
        string firstName,
        string lastName,
        string email,
        string password,
        string phoneNumber,
        EnumUserRole? role,
        CancellationToken cancellationToken)
    {
        var request = new CreateUserRequest(
            firstName, 
            lastName, 
            email, 
            password,
            phoneNumber,
            role ?? EnumUserRole.User);

        var result = await _createUserUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpGet]
    [Route("{email}")]
    public async Task<IActionResult> GetByEmail(
        string email,
        CancellationToken cancellationToken)
    {
        var request = new GetUserByEmailRequest(email);
        
        var result = await _getUserByEmailUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> GetById(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var request = new GetUserByIdRequest(userId);
        
        var result = await _getUserByIdUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var request = new DeleteUserRequest(userId);

        var result = await _deleteUserUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}