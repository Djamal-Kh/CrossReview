using CrossReview.Domain.User;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.GetUserById;

public class GetUserByIdUseCase
{
    private readonly ILogger<GetUserByIdUseCase> _logger;
    private readonly IUserRepository _userRepository;
    
    public GetUserByIdUseCase(
        ILogger<GetUserByIdUseCase> logger, 
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<Result<UserEntity, Errors>> Execute(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return GeneralErrors.NotFound(request.UserId).ToErrors();
        
        _logger.LogInformation("User {UserId} was found", user.Id);
        
        return user;
    }
}