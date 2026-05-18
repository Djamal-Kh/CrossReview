using CrossReview.Application.User.UseCases.DeleteUser;
using CrossReview.Application.User.UseCases.GetAllUsers;
using CrossReview.Application.User.UseCases.GetCurrentUser;
using CrossReview.Application.User.UseCases.GetUserByEmail;
using CrossReview.Application.User.UseCases.GetUserById;
using CrossReview.Application.User.UseCases.Login;
using CrossReview.Application.User.UseCases.RegisterAdmin;
using CrossReview.Application.User.UseCases.RegisterUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = CrossReview.Application.User.UseCases.Login.LoginRequest;

namespace CrossReview.User;

[Route("api/user/")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly DeleteUserUseCase _deleteUserUseCase;
    private readonly GetUserByEmailUseCase _getUserByEmailUseCase;
    private readonly GetUserByIdUseCase _getUserByIdUseCase;
    private readonly RegisterAdminUseCase _registerAdminUseCase;
    private readonly LoginUseCase _loginUseCase;
    private readonly GetCurrentUserUseCase _getCurrentUserUseCase;
    private readonly GetAllUsersUseCase _getAllUsersUseCase;

    public UserController(
        RegisterUserUseCase registerUserUseCase,
        DeleteUserUseCase deleteUserUseCase, 
        GetUserByEmailUseCase getUserByEmailUseCase, 
        GetUserByIdUseCase getUserByIdUseCase,
        RegisterAdminUseCase registerAdminUseCase, 
        LoginUseCase loginUseCase,
        GetCurrentUserUseCase getCurrentUserUseCase, 
        GetAllUsersUseCase getAllUsersUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _deleteUserUseCase = deleteUserUseCase;
        _getUserByEmailUseCase = getUserByEmailUseCase;
        _getUserByIdUseCase = getUserByIdUseCase;
        _registerAdminUseCase = registerAdminUseCase;
        _loginUseCase = loginUseCase;
        _getCurrentUserUseCase = getCurrentUserUseCase;
        _getAllUsersUseCase = getAllUsersUseCase;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(
        string firstName,
        string lastName,
        string email,
        string password,
        string phoneNumber,
        CancellationToken cancellationToken)
    {
        var request = new RegisterUserRequest(
            firstName, 
            lastName, 
            email, 
            password,
            phoneNumber);

        var result = await _registerUserUseCase.Execute(request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    [Route("add-admin")]
    [Authorize(Roles =  "Admin")]
    public async Task<IActionResult> AddAdmin(string firstName,
        string lastName,
        string email,
        string password,
        string phoneNumber,
        CancellationToken cancellationToken)
    {
        var request = new RegisterAdminRequest(
            firstName, 
            lastName, 
            email, 
            password,
            phoneNumber);

        var result = await _registerAdminUseCase.Execute(request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        var request = new LoginRequest(email, password);

        var result = await _loginUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var result = await _getCurrentUserUseCase.Execute(cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllUsersUseCase.Execute();
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("id/{userId:guid}")]
    [Authorize]
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
    
    [HttpGet]
    [Route("{email}")]
    [Authorize]
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
    
    [HttpDelete]
    [Route("delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var request = new DeleteUserRequest(userId);

        var result = await _deleteUserUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok();
    }
}