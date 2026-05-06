using CrossReview.Domain.User;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.GetUserByEmail;

public class GetUserByEmailUseCase
{
    private readonly ILogger<GetUserByEmailUseCase> _logger;
    private readonly IUserRepository _userRepository;
    
    public GetUserByEmailUseCase(
        ILogger<GetUserByEmailUseCase> logger, 
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<Result<UserEntity, Errors>> Execute(GetUserByEmailRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return GeneralErrors.NotFound(request.Email).ToErrors();
        
        _logger.LogInformation("User with email: {Email} was founded", user.Email);

        return user;
    }
}