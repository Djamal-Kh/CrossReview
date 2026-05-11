namespace CrossReview.Application.User;

public sealed record JwtUserModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public List<string> Roles { get; set; }
}
