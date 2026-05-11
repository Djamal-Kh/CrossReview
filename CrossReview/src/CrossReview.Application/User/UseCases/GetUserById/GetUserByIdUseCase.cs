using CrossReview.Application.User.DTO;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.GetUserById;

public class GetUserByIdUseCase
{
    private readonly ILogger<GetUserByIdUseCase> _logger;
    private readonly IIdentityService _identityService;

    public GetUserByIdUseCase(
        ILogger<GetUserByIdUseCase> logger)
    {
        _logger = logger;
    }

    public async Task<Result<UserDto, Errors>> Execute(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetById(request.UserId);

        if (user is null)
            return GeneralErrors.NotFound(request.UserId).ToErrors();

        _logger.LogInformation("User {UserId} was found", user.Id);

        return user;
    }
}
