namespace CrossReview.Application.User;

public interface IJwtProvider
{
    string Generate(JwtUserModel jwt);
}
