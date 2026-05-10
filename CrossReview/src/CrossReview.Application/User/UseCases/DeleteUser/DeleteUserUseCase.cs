using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.DeleteUser;

public class DeleteUserUseCase
{
    private readonly ILogger<DeleteUserUseCase> _logger;
    private readonly IUserRepository _userRepository;
    
    public DeleteUserUseCase(
        ILogger<DeleteUserUseCase> logger,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return GeneralErrors.NotFound(request.UserId).ToErrors();

        var result = await _userRepository.DeleteAsync(user);
        
        _logger.LogInformation("User {UserId} was deleted", user.Id);

        return result;
    }
}