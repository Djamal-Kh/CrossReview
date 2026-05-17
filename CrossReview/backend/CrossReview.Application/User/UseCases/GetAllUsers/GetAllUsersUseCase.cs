using CrossReview.Application.User.DTO;

namespace CrossReview.Application.User.UseCases.GetAllUsers;

public class GetAllUsersUseCase
{
    private readonly IIdentityService _identityService;

    public GetAllUsersUseCase(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<List<UserDto>> Execute()
    {
        return await _identityService.GetAll();
    }
}